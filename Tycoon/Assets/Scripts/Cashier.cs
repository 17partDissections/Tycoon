using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Tycoon.PlayerSystems;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static UnityEditor.Progress;

public class Cashier : MonoBehaviour, IShowcase
{
    private Wallet _wallet;
    private QueueHandler _queueHandler;
    private int _finalCost;
    private bool _isWaiterOnDaDesk;
    private List<BackpackBuyer> _waitingBuyers = new List<BackpackBuyer>();
    [SerializeField] private Transform _startOfQueuePosition;
    private Vector3 _lastPositionInQueue;
    [SerializeField] private DirectionOfQueue _directionOfQueue;

    public int PplInQueueAmount { get; set; }
    public Transform FirstPointOfQueue { get => _startOfQueuePosition; set => _startOfQueuePosition = value; }

    public event Action BuyerHasGoneSignal;

    [Inject]
    private void Construct(Storage storage, Wallet wallet, QueueHandler queueHandler)
    {
        _lastPositionInQueue = _startOfQueuePosition.position;
        storage.AddCashierPosition(gameObject.transform);
        _wallet = wallet;
        _queueHandler = queueHandler;
        storage.AddItem2Aviable(ItemName.Cashier, _lastPositionInQueue);
        storage.AddQueueDirection(ItemName.Cashier, _directionOfQueue);
        storage.AddShowcaseOrCashier(ItemName.Cashier, this);


    }


    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Backpack>(out Backpack backpack);
        if (backpack is BackpackBuyer && !backpack.IsBackpackNotFull() && _isWaiterOnDaDesk)
        {
            SellItem(backpack);
        }
        else if (backpack is BackpackBuyer && !backpack.IsBackpackNotFull() && _isWaiterOnDaDesk == false)
        {
            _waitingBuyers.Add(backpack as BackpackBuyer);

        }
        else if (other.GetComponent<PlayerMovement>() != null)
        {
            _isWaiterOnDaDesk = true;
            StartCoroutine(WaiterWorkCoroutine());
            _wallet.CoinsAmount += _finalCost;
            _finalCost = 0;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<Backpack>(out Backpack backpack);
        if (backpack is BackpackWorker)
        {
            _isWaiterOnDaDesk = false;
            StopAllCoroutines();


        }

    }
    private void SellItem(Backpack backpack)
    {
        List<Item> items = backpack.GiveItemsList();
        foreach (Item item in items)
        {
            _finalCost += item.Price;
        }
        //графическое добавление денег, как подбираемые объекты

        backpack.DestroyAllItems();
        _wallet.CoinsAmount += _finalCost;
        _finalCost = 0;
        backpack.TryGetComponent<BuyerStateMachine>(out BuyerStateMachine buyerStateMachine);
        //buyerStateMachine.MovingForwardInQueue();

        _queueHandler.MooveByersInQueue(ItemName.Cashier);
        buyerStateMachine.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.RunAwayState);
        BuyerHasGoneSignal?.Invoke();

    }

    private IEnumerator WaiterWorkCoroutine()
    {
        while (_isWaiterOnDaDesk == true && _waitingBuyers.Count > 0)
        {
            SellItem(_waitingBuyers[0]);
            _waitingBuyers.RemoveAt(0);
            yield return new WaitForSeconds(2.5f);
        }

    }

}
