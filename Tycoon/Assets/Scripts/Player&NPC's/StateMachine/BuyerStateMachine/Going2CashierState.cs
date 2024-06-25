using UnityEngine;
using Zenject;

public class Going2CashierState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    public Going2CashierState(BuyerStateMachine.BuyerStates state,
    BuyerStateMachine CurrentStateMachine) : base(state)
    {
        _stateMachine = CurrentStateMachine;
    }

    public override void Enter2State()
    {
        Debug.Log("вошел в состояние");
        _stateMachine.Agent.SetDestination(_stateMachine.Storage.CashierPosition.position);
    }

    public override void Exit2State()
    {
    }

    public override void UpdateState()
    {
    }
}
