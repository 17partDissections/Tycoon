
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Tycoon.PlayerSystems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class ShowcaseAbstraction<T> : MonoBehaviour, IShowcase where T : Item
{
    public int ShowcasePrice;

    [SerializeField] private List<T> _showcaseInventory;
    [SerializeField] private List<BackpackBuyer> BuyerEnteredTrigger = new List<BackpackBuyer>();
    private T _item;
    private int _enabledItems4Buyers;
    private EventBus _eventbus;
    private Storage _storage;
    private Wallet _playerWallet;
    private QueueHandler _queueHandler;
    private AudioSources _audioSources;
    private Backpack _backpackAbstraction;
    private bool _buyed = false;
    private FabricsNShowcasesCanvas _canvas;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _text;
    public Transform _startOfQueuePosition;
    private Vector3 _lastPositionInQueue;
    [SerializeField] private DirectionOfQueue _directionOfQueue;

    public int PplInQueueAmount { get; set; }
    public Transform FirstPointOfQueue { get => _startOfQueuePosition; set => _startOfQueuePosition = value; }

    public event Action BuyerHasGoneSignal;

    private void Awake()
    {
        _lastPositionInQueue = _startOfQueuePosition.position;
        _item = _showcaseInventory[0];
        _storage.AddItem2Aviable(_item.ItemName, _lastPositionInQueue);
        _storage.AddQueueDirection(_item.ItemName, _directionOfQueue);
        _storage.AddShowcaseOrCashier(_item.ItemName, this);
        _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, ShowcasePrice);

    }
    [Inject]
    private void Construct(EventBus bus, Storage storage, Wallet wallet, QueueHandler queueHandler, AudioSources audioSources)
    {
        _eventbus = bus;
        _storage = storage;
        _playerWallet = wallet;
        _queueHandler = queueHandler;
        _audioSources = audioSources;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_buyed)
        {
            if (other.TryGetComponent<Backpack>(out Backpack backpackAbstraction))
            {
                _backpackAbstraction = backpackAbstraction;
                switch (backpackAbstraction.GetType().Name)
                {
                    case nameof(BackpackWorker):
                        BackpackWasWorker();
                        break;
                    case nameof(BackpackBuyer):
                        BackpackWasBuyer(backpackAbstraction);
                        break;
                }
            }

        }
        else if (other.GetComponent<PlayerHotkeys>() != null)
        {
            BuyShowcase();
        }
        else if (other.TryGetComponent<Backpack>(out Backpack backpackAbstraction) && backpackAbstraction.GetType().Name == nameof(BackpackBuyer))
        {
            BackpackWasBuyer(backpackAbstraction);
        }
    }
    private void BackpackWasBuyer(Backpack backpackAbstraction)
    {

        if (backpackAbstraction.TryGetComponent<BuyerStateMachine>(out BuyerStateMachine stateMachine))
        {
            var sameObjects = stateMachine.WannaBuy.Where(x => x == _item.ItemName);
            List<ItemName> RepeatingItemsList = new List<ItemName>(sameObjects);

            var backpackBuyer = backpackAbstraction as BackpackBuyer;
            backpackBuyer.RepeatingItems.Clear();
            backpackBuyer.RepeatingItems.AddRange(RepeatingItemsList);

            if (RepeatingItemsList.Count() > 0)
            {
                BuyerEnteredTrigger.Add(backpackAbstraction as BackpackBuyer);
                GiveItems2Buyer(backpackAbstraction, backpackBuyer.RepeatingItems);

            }
        }
    }

    public void BuyShowcase()
    {
        if (_playerWallet.Trying2BuySmthng(ShowcasePrice) == true)
        {
            _buyed = true;
            _audioSources.PlaySound(_audioSources.Coin);
            _canvas.OnlyIcon();
            _eventbus.StageSignal.Invoke(2);
        }
    }

    private void GiveItems2Buyer(Backpack backpackAbstraction, List<ItemName> RepeatingItemsList)
    {
        if (_enabledItems4Buyers > 0)
        {
            GiveItems2Buyer(backpackAbstraction);
            if (RepeatingItemsList.Count() == 0)
            {
                BuyerEnteredTrigger.Remove(backpackAbstraction as BackpackBuyer);
            }
            else
            {
                GiveItems2Buyer(backpackAbstraction, RepeatingItemsList);
            }
        }
    }

    private void GiveItems2Buyer(Backpack backpackAbstraction)
    {
        if (backpackAbstraction.IsBackpackNotFull() != false)
        {
            var itemCopyOfGameObject = Instantiate(_item, gameObject.transform);
            var backpackBuyer = backpackAbstraction as BackpackBuyer;
            backpackBuyer.SaveItem(itemCopyOfGameObject);
            _enabledItems4Buyers--;
            backpackBuyer.RepeatingItems.RemoveAt(0);
            //GiveItems2Buyer(backpackAbstraction, backpackBuyer.RepeatingItems);
            var ActiveItems = _showcaseInventory.FindAll(x => x.isActiveAndEnabled);
            ActiveItems[0].gameObject.SetActive(false);
            backpackBuyer.WannaBuy.Remove(_item.ItemName);
            if (backpackAbstraction.IsBackpackNotFull() == false)
                RemovingFromBuyerEnteredTrigger(backpackAbstraction as BackpackBuyer);
        }
        else
        {
            RemovingFromBuyerEnteredTrigger(backpackAbstraction as BackpackBuyer);
        }

    }

    private void RemovingFromBuyerEnteredTrigger(BackpackBuyer backpackBuyer)
    {
        BuyerStateMachine buyerStateMachine = backpackBuyer.GetComponent<BuyerStateMachine>();
        BuyerEnteredTrigger.Remove(backpackBuyer);
        if (backpackBuyer.WannaBuy.Count == 0)
        {
            buyerStateMachine.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.Going2CashierState);
            //buyerStateMachine.Storage.ChangePositionInQueue(buyerStateMachine.WannaBuy[0], false);
            _queueHandler.MoveBuyersInQueue(_item.ItemName);
            //buyerStateMachine.MovingForwardInQueue();
        }
        else
        {

            buyerStateMachine.Agent.SetDestination(buyerStateMachine.GetWannaBuyObjPosition(backpackBuyer.WannaBuy[0]));
            //buyerStateMachine.Storage.ChangePositionInQueue(buyerStateMachine.WannaBuy[0], false);
            //buyerStateMachine.MovingForwardInQueue();
            buyerStateMachine.Subscribe2NewItem(_item.ItemName);
            _queueHandler.MoveBuyersInQueue(_item.ItemName);

        }
        BuyerHasGoneSignal?.Invoke();
    }


    private void BackpackWasWorker()
    {
        var nonActiveItemsBeforeAdding = _showcaseInventory.FindAll(x => !x.isActiveAndEnabled);
        if (nonActiveItemsBeforeAdding.Count() == 0)
        {
            return;
        }
        var trying2FindItem = _backpackAbstraction.RemoveItem(_item.ItemName);
        if (trying2FindItem != null)
        {
            var nonActiveItems = _showcaseInventory.FindAll(x => !x.isActiveAndEnabled);
            int randomNumber = UnityEngine.Random.Range(0, nonActiveItems.Count);
            _enabledItems4Buyers++;
            nonActiveItems[randomNumber].gameObject.SetActive(true);
            BackpackWasWorker();
            if (BuyerEnteredTrigger.Count == 0)
            {
                return;
            }
            else
                GiveItems2Buyer(BuyerEnteredTrigger[0]);



        }
    }



}
public enum DirectionOfQueue
{
    East,
    West,
    South,
    North
}


