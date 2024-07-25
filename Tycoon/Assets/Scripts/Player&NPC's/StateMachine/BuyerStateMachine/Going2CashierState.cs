using UnityEngine;
using Zenject;

public class Going2CashierState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    public Going2CashierState(BuyerStateMachine.BuyerStates state,
    BuyerStateMachine currentStateMachine) : base(state)
    {
        _stateMachine = currentStateMachine;
    }

    public override void Enter2State()
    {
        _stateMachine.Animator.SetBool(_stateMachine.waiting, false);
        _stateMachine.CurrentItemInList = ItemName.Cashier;
        _stateMachine.Agent.SetDestination(_stateMachine.Storage.GetPosition(ItemName.Cashier));
        _stateMachine.Storage.ChangePositionInQueue(ItemName.Cashier, true);
        _stateMachine.Storage.IShowcaseDictionary[ItemName.Cashier].BuyerHasGoneSignal += _stateMachine.MovingForwardInQueue;
    }

    public override void Exit2State()
    {
        _stateMachine.Storage.IShowcaseDictionary[ItemName.Cashier].BuyerHasGoneSignal -= _stateMachine.MovingForwardInQueue;
    }

    public override void UpdateState()
    {
    }
}
