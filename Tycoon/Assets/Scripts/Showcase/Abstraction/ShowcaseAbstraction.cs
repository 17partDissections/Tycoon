
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Tycoon.PlayerSystems;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public abstract class ShowcaseAbstraction : MonoBehaviour, IBuyable, IShowcase
{
    public int ShowcasePrice;
    
    [SerializeField] protected List<Item> ShowcaseInventory;
    [SerializeField] protected List<BackpackBuyer> BuyerEnteredTrigger = new List<BackpackBuyer>();
    public Item Item;
    public int EnabledItems4Buyers;
    private EventBus _eventbus;
    private Storage _storage;
    private Wallet _playerWallet;
    private QueueHandler _queueHandler;
    private AudioHandler _audioHandler;
    protected Backpack BackpackAbstraction;
    [HideInInspector] public bool Buyed = false;
    private FabricsNShowcasesCanvas _canvas;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _buyCircle;
    public Transform _startOfQueuePosition;
    private Vector3 _lastPositionInQueue;
    [SerializeField] private DirectionOfQueue _directionOfQueue;
    [SerializeField] private AudioClip _purchase;
    public Image BuyCircle;
    private bool _isPlayerInTrigger;
    [SerializeField] private int _stage;
    private SaveSystem _saveSystem;

    public int PplInQueueAmount { get; set; }
    public Transform FirstPointOfQueue { get => _startOfQueuePosition; set => _startOfQueuePosition = value; }

    public event Action BuyerHasGoneSignal;

    private void Awake()
    {
        
        _lastPositionInQueue = _startOfQueuePosition.position;
        Item = ShowcaseInventory[0];
        _storage.AddQueueDirection(Item.ItemName, _directionOfQueue);
        _storage.AddShowcaseOrCashier(Item.ItemName, this);
        _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, ShowcasePrice, _buyCircle);

    }
    private void OnDestroy()
    {
        YandexGame.SaveProgress();
    }
    [Inject]
    private void Construct(SaveSystem saveSys, EventBus bus, Storage storage, Wallet wallet, QueueHandler queueHandler, AudioHandler audioSources)
    {
        _saveSystem = saveSys;
        _eventbus = bus;
        _storage = storage;
        _playerWallet = wallet;
        _queueHandler = queueHandler;
        _audioHandler = audioSources;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _isPlayerInTrigger = true;
        if (Buyed)
        {
            if (other.TryGetComponent<Backpack>(out Backpack backpackAbstraction))
            {
                BackpackAbstraction = backpackAbstraction;
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
        else if (other.TryGetComponent<Backpack>(out Backpack backpackAbstraction) && backpackAbstraction.GetType().Name == nameof(BackpackBuyer))
        {
            BackpackWasBuyer(backpackAbstraction);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(!Buyed)
            if (other.tag == "Player")
                BuyingProcess();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _canvas.BuyCircle.fillAmount = 0;
            _isPlayerInTrigger = false;

        }
    }
    private void BackpackWasBuyer(Backpack backpackAbstraction)
    {

        if (backpackAbstraction.TryGetComponent<BuyerStateMachine>(out BuyerStateMachine stateMachine))
        {
            
            var sameObjects = stateMachine.WannaBuy.Where(x => x == Item.ItemName);
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
            Buyed = true;
            _audioHandler.PlaySFX(_purchase);
            _canvas.OnlyIcon();
            _storage.AddItem2Aviable(Item.ItemName, _lastPositionInQueue);
            _eventbus.StageSignal.Invoke(_stage);
            _saveSystem.SaveShowcase(this);
        }
    }
    public void BuyShowcaseTroughLoader()
    {
        Buyed = true;
        _canvas.OnlyIcon();
        _storage.AddItem2Aviable(Item.ItemName, _lastPositionInQueue);
        _eventbus.StageSignal.Invoke(_stage);
    }

    private void GiveItems2Buyer(Backpack backpackAbstraction, List<ItemName> RepeatingItemsList)
    {
        if (EnabledItems4Buyers > 0)
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

    protected void GiveItems2Buyer(Backpack backpackAbstraction)
    {
        var backpackBuyer = backpackAbstraction as BackpackBuyer;
        if (backpackAbstraction.IsBackpackFull() == false && backpackBuyer.RepeatingItems.Count() > 0)
        {
            var itemCopyOfGameObject = Instantiate(Item, gameObject.transform);
            backpackBuyer.SaveItem(itemCopyOfGameObject);
            EnabledItems4Buyers--;
            backpackBuyer.RepeatingItems.RemoveAt(0);
            _saveSystem.SaveShowcase(this);
            var ActiveItems = ShowcaseInventory.FindAll(x => x.isActiveAndEnabled);
            ActiveItems[0].gameObject.SetActive(false);
            backpackBuyer.WannaBuy.Remove(Item.ItemName);
            if (backpackAbstraction.IsBackpackFull() == true || backpackBuyer.RepeatingItems.Count() == 0)
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
            Debug.Log(backpackBuyer.WannaBuy.Count);
            buyerStateMachine.ChangeStateFromMachine(BuyerStateMachine.BuyerStates.Going2CashierState);
            //buyerStateMachine.Storage.ChangePositionInQueue(buyerStateMachine.WannaBuy[0], false);
            _queueHandler.MoveBuyersInQueue(Item.ItemName);
            //buyerStateMachine.MovingForwardInQueue();
        }
        else
        {

            buyerStateMachine.Agent.SetDestination(buyerStateMachine.GetWannaBuyObjPosition(backpackBuyer.WannaBuy[0]));
            buyerStateMachine.Subscribe2NewItem(backpackBuyer.WannaBuy[0]);
            _queueHandler.MoveBuyersInQueue(Item.ItemName);

        }
        BuyerHasGoneSignal?.Invoke();
    }


    protected virtual void BackpackWasWorker()
    {
        var nonActiveItemsBeforeAdding = ShowcaseInventory.FindAll(x => !x.isActiveAndEnabled);
        if (nonActiveItemsBeforeAdding.Count() == 0)
        {
            return;
        }
        var trying2FindItem = BackpackAbstraction.RemoveItem(Item.ItemName);
        if (trying2FindItem != null)
        {
            var nonActiveItems = ShowcaseInventory.FindAll(x => !x.isActiveAndEnabled);
            int randomNumber = UnityEngine.Random.Range(0, nonActiveItems.Count);
            EnabledItems4Buyers++;
            nonActiveItems[randomNumber].gameObject.SetActive(true);
            BackpackWasWorker();
            if (BuyerEnteredTrigger.Count == 0)
            {
                return;
            }
            else
                GiveItems2Buyer(BuyerEnteredTrigger[0]);
        }
        _saveSystem.SaveShowcase(this);
    }
    public void LoadItems()
    {
        var nonActiveItems = ShowcaseInventory.FindAll(x => !x.isActiveAndEnabled);
        int randomNumber = UnityEngine.Random.Range(0, nonActiveItems.Count);
        EnabledItems4Buyers++;
        nonActiveItems[randomNumber].gameObject.SetActive(true);
    }

    public void BuyingProcess()
    {
        if (_canvas.BuyCircle.fillAmount < 1)
            _canvas.BuyCircle.fillAmount += Time.deltaTime;
        else
        {
            _canvas.BuyCircle.fillAmount = 0;
            BuyShowcase();
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


