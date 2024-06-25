
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class ShowcaseAbstraction<T> : MonoBehaviour where T : Item
{
    public int ShowcasePrice;

    [SerializeField] private List<T> _showcaseInventory;
    [SerializeField] private List<BackpackBuyer> BuyerEnteredTrigger = new List<BackpackBuyer>();
    private T _item;
    private int _enabledItems4Buyers;
    private EventBus _eventbus;
    private Storage _storage;
    private Wallet _playerWallet;
    private Backpack _backpackAbstraction;
    private bool _buyed = false;
    private FabricsNShowcasesCanvas _canvas;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _text;

    private void AddItem()
    {

    }
    //private T RemoveItem()
    //{
    //    _enabledItems4Buyers--;
    //}

    private void Awake()
    {

        _item = _showcaseInventory[0];
        _storage.AddItem2Aviable(_item.ItemName, transform);
        _canvas = new FabricsNShowcasesCanvas(_itemIcon, _text, ShowcasePrice);

        //_storage.AddItem2Aviable(ItemName.Watermelon, transform);
        //_storage.AddItem2Aviable(ItemName.Banana, transform);

    }
    [Inject]
    private void Construct(EventBus bus, Storage storage, Wallet wallet)
    {
        _eventbus = bus;
        _storage = storage;
        _playerWallet = wallet;
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
                        if (backpackAbstraction.TryGetComponent<BuyerStateMachine>(out BuyerStateMachine stateMachine))
                        {
                            var sameObjects = stateMachine.WannaBuy.Where(x => x == _item.ItemName);
                            List<ItemName> RepeatingItemsList = new List<ItemName>(sameObjects);

                            var backpackBuyer = backpackAbstraction as BackpackBuyer;
                            backpackBuyer.RepeatingItems.Clear();
                            backpackBuyer.RepeatingItems.AddRange(RepeatingItemsList);
                            Debug.Log(backpackBuyer.RepeatingItems.Count);
                            if (RepeatingItemsList.Count() > 0)
                            {
                                BuyerEnteredTrigger.Add(backpackAbstraction as BackpackBuyer);

                                GiveItems2Buyer(backpackAbstraction, backpackBuyer.RepeatingItems);

                            }


                        }
                        break;
                }
            }

        }
        else
        {
            BuyShowcase();
        }
    }
    public void BuyShowcase()
    {
        if (_playerWallet.Trying2BuySmthng(ShowcasePrice) == true)
        {
            _buyed = true;
            _canvas.OnlyIcon();
            _eventbus.StageSignal.Invoke(2);
        }

    }

    private void GiveItems2Buyer(Backpack backpackAbstraction, List<ItemName> RepeatingItemsList)
    {
        if (_enabledItems4Buyers > 0)
        {
            CopyObject(backpackAbstraction);
            Debug.Log(RepeatingItemsList.Count);
            if (RepeatingItemsList.Count() == 0)
            {
                BuyerEnteredTrigger.Remove(backpackAbstraction as BackpackBuyer);
                //next state
            }
            else  
            {
                GiveItems2Buyer(backpackAbstraction, RepeatingItemsList);
            }
        }
    }

    private void CopyObject(Backpack backpackAbstraction)
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
        }
        else
        {
            buyerStateMachine.Agent.SetDestination(buyerStateMachine.GetWannaBuyObjPosition(backpackBuyer.WannaBuy[0]).position);
            
        }
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
            int randomNumber = Random.Range(0, nonActiveItems.Count);
            _enabledItems4Buyers++;
            nonActiveItems[randomNumber].gameObject.SetActive(true);
            BackpackWasWorker();
            if (BuyerEnteredTrigger.Count == 0)
            {
                return;
            }
            CopyObject(BuyerEnteredTrigger[0]);
            
            if (BuyerEnteredTrigger.Count > 0 && BuyerEnteredTrigger[0].LeftItems(_item.ItemName) == 0)
            {
                
                BuyerEnteredTrigger.RemoveAt(0);
            }


        }
    }
    


}
