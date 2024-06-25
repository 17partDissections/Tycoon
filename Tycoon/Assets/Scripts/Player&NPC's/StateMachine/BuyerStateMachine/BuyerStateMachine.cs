using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;


public class BuyerStateMachine : StateMachineController<BuyerStateMachine.BuyerStates>
{
    public Animator Animator;
    public Transform WayPoint;
    public NavMeshAgent Agent;
    public BackpackBuyer BackpackBuyer;
    public Storage Storage { get; private set; }

    public ItemName[] WannaBuy  = new ItemName[3];

    
    


    public enum BuyerStates 
    {
        StartState,
        ThinkinState,
        Going2ItemState,
        Going2CashierState,
        Waiting4DaWaiter,
        RunAway,
    }


    [HideInInspector] public int Idle;
    [HideInInspector] public int Walking;



    private void Start()
    {
        Idle = Animator.StringToHash("idle");
        Walking = Animator.StringToHash("walking");

       StartState StartState = new StartState(BuyerStates.StartState, this);
        States.Add(BuyerStates.StartState,StartState);
        ThinkinState ThinkinState = new ThinkinState(BuyerStates.ThinkinState, this);
        States.Add(BuyerStates.ThinkinState, ThinkinState);
        Going2ItemState Going2ItemState = new Going2ItemState(BuyerStates.Going2ItemState, this);
        States.Add(BuyerStates.Going2ItemState, Going2ItemState);
        Going2CashierState Going2CashierState = new Going2CashierState(BuyerStates.Going2CashierState, this);
        States.Add(BuyerStates.Going2CashierState, Going2CashierState);
        StartMachine(BuyerStates.StartState);

        foreach (var item in States)
        {
            Debug.Log(item.Key.ToString());
        }
    }
    public void Init(Storage storage)
    {
        Storage = storage;
        Debug.Log(Storage);

    }
    public void ChangeStateFromMachine(BuyerStates state)
    {
        ChangeActionFromChildren(state);
    }
    public Transform GetWannaBuyObjPosition(ItemName item2find)
    {
        Transform foundPosition = Storage.GetPosition(item2find);
        return foundPosition;
    }





}

