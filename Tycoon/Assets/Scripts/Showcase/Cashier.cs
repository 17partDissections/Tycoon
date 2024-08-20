using System;
using System.Collections;
using System.Collections.Generic;
using Tycoon.PlayerSystems;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class Cashier : MonoBehaviour, IShowcase
{
    private Wallet _wallet;
    private QueueHandler _queueHandler;
    private int _finalCost;
    private bool _isPlayerOnDaDesk;
    private bool _isWorkerOnDaDesk;
    private List<BackpackBuyer> _waitingBuyers = new List<BackpackBuyer>();
    [SerializeField] private Transform _startOfQueuePosition;
    private Vector3 _lastPositionInQueue;
    [SerializeField] private DirectionOfQueue _directionOfQueue;
    [SerializeField] private List<GameObject> _moneyStages;
    [SerializeField] private List<Transform> _moneyObj;
    private Coroutine _cashCoroutine;
    private Coroutine _cashierWorkCoroutine;
    private AudioHandler _audioHandler;
    [SerializeField] private AudioClip _purchase;




    public int PplInQueueAmount { get; set; }
    public Transform FirstPointOfQueue { get => _startOfQueuePosition; set => _startOfQueuePosition = value; }

    public event Action BuyerHasGoneSignal;

    [Inject]
    private void Construct(Storage storage, Wallet wallet, QueueHandler queueHandler, AudioHandler audioSources)
    {
        _lastPositionInQueue = _startOfQueuePosition.position;
        storage.AddCashierPosition(gameObject.transform);
        _wallet = wallet;
        _queueHandler = queueHandler;
        _audioHandler = audioSources;
        storage.AddItem2Aviable(ItemName.Cashier, _lastPositionInQueue);
        storage.AddQueueDirection(ItemName.Cashier, _directionOfQueue);
        storage.AddShowcaseOrCashier(ItemName.Cashier, this);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHotkeys>() != null)
        {
            _isPlayerOnDaDesk = true;
            if (!_isWorkerOnDaDesk)
                _cashierWorkCoroutine = StartCoroutine(WaiterWorkCoroutine());
            if (_finalCost > 0)
                StartCoroutine(BringDaMoney(other));
        }
        else if (other.TryGetComponent<WorkerStateMachine>(out WorkerStateMachine workerStateMachine))
        {
            if (workerStateMachine.WorkerWork == Works.CashierWorker)
            {
                _isWorkerOnDaDesk = true;
                if (_cashierWorkCoroutine != null)
                {
                    StopCoroutine(_cashierWorkCoroutine);
                    _cashierWorkCoroutine = StartCoroutine(WaiterWorkCoroutine());
                } 
            }

        }
        if (other.TryGetComponent<Backpack>(out Backpack backpack))
            if (backpack is BackpackBuyer && backpack.IsBackpackFull())
            {
                if (_isWorkerOnDaDesk || _isPlayerOnDaDesk)
                    SellItem(backpack);
                else
                    _waitingBuyers.Add(backpack as BackpackBuyer);
            }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Backpack>(out Backpack backpack))
            if (other.GetComponent<PlayerHotkeys>() != null)
            {
                _isPlayerOnDaDesk = false;
                if (!_isWorkerOnDaDesk)
                StopCoroutine(_cashierWorkCoroutine);
            }
            else if (other.TryGetComponent<WorkerStateMachine>(out WorkerStateMachine workerStateMachine))
            {
                if(workerStateMachine.WorkerWork == Works.CashierWorker)
                {
                _isWorkerOnDaDesk = false;
                StopCoroutine(_cashierWorkCoroutine);
                }

            }
    }
    private void SellItem(Backpack backpack)
    {
        foreach (Item item in backpack.GiveItemsList())
            _finalCost += item.Price;

        //графическое добавление денег, как подбираемые объекты
        switch (_finalCost)
        {
            //case int i when i < 0 && > 5
            case > 0 and <= 15:
                _moneyStages[0].SetActive(true);
                Debug.Log("Stage= " + 0);
                break;
            case > 15 and <= 30:
                _moneyStages[1].SetActive(true);
                Debug.Log("Stage= " + 1);
                break;
            case > 30 and <= 200:
                _moneyStages[2].SetActive(true);
                Debug.Log("Stage= " + 2);
                break;
        }
        backpack.DestroyAllItems();
        _audioHandler.PlaySFX(_purchase);
        backpack.TryGetComponent<BuyerStateMachine>(out BuyerStateMachine buyerStateMachine);
        _queueHandler.MoveBuyersInQueue(ItemName.Cashier);
        buyerStateMachine.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.RunAwayState);
    }

    private IEnumerator WaiterWorkCoroutine()
    {
        while (true)
        {
            if (_waitingBuyers.Count > 0)
            {
                SellItem(_waitingBuyers[0]);
                _waitingBuyers.RemoveAt(0);
            }
            yield return new WaitForSeconds(2.5f);
        }
    }
    private IEnumerator BringDaMoney(Collider playerCollider)
    {
        playerCollider.gameObject.TryGetComponent(out PlayerHotkeys player);
        var oldMoneyObjPosition = _moneyObj[0].transform.position;
        _moneyObj[0].transform.parent.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        foreach (var item in _moneyObj)
        {
            item.gameObject.SetActive(true);
            sequence
                .Append(item.DOMove(player.transform.position, 0.2f).SetEase(Ease.Linear));
            yield return new WaitForSeconds(0.2f);
            item.gameObject.SetActive(false);
            item.transform.position = oldMoneyObjPosition;
        }
        _moneyObj[0].transform.parent.gameObject.SetActive(false);
        _wallet.CoinsAmount += _finalCost;
        foreach (var item in _moneyStages)
            item.gameObject.SetActive(false);
        _finalCost = 0;
    }

}
