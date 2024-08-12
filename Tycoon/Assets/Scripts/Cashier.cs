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
    private bool _isWorkerOnDaDesk;
    private List<BackpackBuyer> _waitingBuyers = new List<BackpackBuyer>();
    [SerializeField] private Transform _startOfQueuePosition;
    private Vector3 _lastPositionInQueue;
    [SerializeField] private DirectionOfQueue _directionOfQueue;
    [SerializeField] private List<GameObject> _moneyStages;
    [SerializeField] private GameObject _moneyObj;
    private Coroutine _cashCoroutine;
    private AudioSources _audioSources;

    
    

    public int PplInQueueAmount { get; set; }
    public Transform FirstPointOfQueue { get => _startOfQueuePosition; set => _startOfQueuePosition = value; }

    public event Action BuyerHasGoneSignal;

    [Inject]
    private void Construct(Storage storage, Wallet wallet, QueueHandler queueHandler, AudioSources audioSources)
    {
        _lastPositionInQueue = _startOfQueuePosition.position;
        storage.AddCashierPosition(gameObject.transform);
        _wallet = wallet;
        _queueHandler = queueHandler;
        _audioSources = audioSources;
        storage.AddItem2Aviable(ItemName.Cashier, _lastPositionInQueue);
        storage.AddQueueDirection(ItemName.Cashier, _directionOfQueue);
        storage.AddShowcaseOrCashier(ItemName.Cashier, this);
        

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHotkeys>() != null)
        {
            _isWorkerOnDaDesk = true;
            StartCoroutine(WaiterWorkCoroutine());
            if(_finalCost > 0)
                StartCoroutine(BringDaMoney(other));
        }
        other.TryGetComponent<Backpack>(out Backpack backpack);
        if (backpack is BackpackBuyer && !backpack.IsBackpackNotFull())
        {
            List<Item> items = backpack.GiveItemsList();
            foreach (Item item in items)
            {
                _finalCost += item.Price;
            }
            Debug.Log(_finalCost);
            if (_isWorkerOnDaDesk)
            {
                StartCoroutine(BringDaMoney(other));
                SellItem(backpack);
                if (_finalCost > 0)
                    StartCoroutine(BringDaMoney(other));
            }
            else
                _waitingBuyers.Add(backpack as BackpackBuyer);
        }



    }
    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<Backpack>(out Backpack backpack);
        if (backpack is BackpackWorker)
        {
            _isWorkerOnDaDesk = false;
            //StopAllCoroutines();


        }

    }
    private void SellItem(Backpack backpack)
    {

        //графическое добавление денег, как подбираемые объекты
        switch (_finalCost)
        {
            //case int i when i < 0 and > 5
            case > 0 and <= 15:
                _moneyStages[0].SetActive(true);
                Debug.Log(0);
                break;
            case > 15 and <= 30:
                _moneyStages[1].SetActive(true);
                Debug.Log(1);
                break;
            case > 30 and <= 200:
                _moneyStages[2].SetActive(true);
                Debug.Log(2);
                break;
        }
        backpack.DestroyAllItems();
        _audioSources.PlaySound(_audioSources.Purchase);
        _wallet.CoinsAmount += _finalCost;
        _finalCost = 0;
        backpack.TryGetComponent<BuyerStateMachine>(out BuyerStateMachine buyerStateMachine);
        //buyerStateMachine.MovingForwardInQueue();

        _queueHandler.MoveBuyersInQueue(ItemName.Cashier);
        buyerStateMachine.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.RunAwayState);
        BuyerHasGoneSignal?.Invoke();

    }

    private IEnumerator WaiterWorkCoroutine()
    {
        while (_isWorkerOnDaDesk == true && _waitingBuyers.Count > 0)
        {
            SellItem(_waitingBuyers[0]);
            _waitingBuyers.RemoveAt(0);
            yield return new WaitForSeconds(2.5f);
        }

    }
    private IEnumerator BringDaMoney(Collider playerCollider)
    {
            var oldMoneyObjPosition = _moneyObj.transform.position;
            _moneyObj.SetActive(true);
            Sequence sequence = DOTween.Sequence();
        var duration = 1;
        sequence.Append(_moneyObj.transform.DOMove(playerCollider.transform.position, duration).SetEase(Ease.Linear));
            Debug.Log(playerCollider.transform.position.ToString());
            yield return new WaitForSeconds(duration);
            Debug.Log("подождал");
            _moneyObj.gameObject.SetActive(false);
            _wallet.CoinsAmount += _finalCost;
            _finalCost = 0; 
            _moneyObj.transform.position = oldMoneyObjPosition;
        
    }

}
