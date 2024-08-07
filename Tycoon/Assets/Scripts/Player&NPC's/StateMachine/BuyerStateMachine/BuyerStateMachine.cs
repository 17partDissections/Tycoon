using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;
using Zenject;


public class BuyerStateMachine : StateMachineController<BuyerStateMachine.BuyerStates>
{
    public ObjectPool<BuyerStateMachine> ObjectPool;
    public Animator Animator;
    public Vector3 WayPoint;
    public NavMeshAgent Agent;
    public BackpackBuyer BackpackBuyer;
    public EventBus EventBus;
    public QueueHandler QueueHandler;

    public Storage Storage { get; private set; }

    public ItemName[] WannaBuy = new ItemName[3];
    public ItemName CurrentItemInList;
    public int NumerationOfBuyerInQueue;
    private bool _isBuyerLast = true;
    public enum BuyerStates
    {
        StartState,
        ThinkinState,
        Going2ItemState,
        Going2CashierState,
        Waiting4DaWaiterState,
        RunAwayState,
    }


    [HideInInspector] public int waiting;



    private void Awake()
    {
        waiting = Animator.StringToHash("Waiting");

        StartState StartState = new StartState(BuyerStates.StartState, this);
        States.Add(BuyerStates.StartState, StartState);
        ThinkinState ThinkinState = new ThinkinState(BuyerStates.ThinkinState, this);
        States.Add(BuyerStates.ThinkinState, ThinkinState);
        Going2ItemState Going2ItemState = new Going2ItemState(BuyerStates.Going2ItemState, this);
        States.Add(BuyerStates.Going2ItemState, Going2ItemState);
        Going2CashierState Going2CashierState = new Going2CashierState(BuyerStates.Going2CashierState, this);
        States.Add(BuyerStates.Going2CashierState, Going2CashierState);
        RunAwayState RunAwayState = new RunAwayState(BuyerStates.RunAwayState, this);
        States.Add(BuyerStates.RunAwayState, RunAwayState);
        StartMachine(BuyerStates.StartState);
    }
    [Inject]
    public void Init(Storage storage, EventBus eventBus, QueueHandler queueHandler)
    {
        Storage = storage;
        EventBus = eventBus;
        QueueHandler = queueHandler;
    }
    public void ChangeStateFromMachine(BuyerStates state)
    {
        ChangeActionFromChildren(state);
    }
    public Vector3 GetWannaBuyObjPosition(ItemName item2find)
    {
        Vector3 foundPosition = QueueHandler.CalcPositionInQueue(Storage.DirectionOfQueue[item2find],
            Storage.IShowcaseDictionary[item2find].FirstPointOfQueue, Storage.IShowcaseDictionary[item2find].PplInQueueAmount);
        return foundPosition;
    }

    public void Subscribe2NewItem(ItemName itemName)
    {
        Going2ItemState going2ItemState = States[BuyerStates.Going2ItemState] as Going2ItemState;
        going2ItemState.Subscribe2NewItem(itemName);
    }




}

