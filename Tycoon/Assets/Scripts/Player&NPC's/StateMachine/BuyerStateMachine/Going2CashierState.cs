using UnityEngine;
using Zenject;

public class Going2CashierState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    private Transform _cashierPos;
    public Going2CashierState(BuyerStateMachine.BuyerStates state,
    BuyerStateMachine CurrentStateMachine) : base(state)
    {
        _stateMachine = CurrentStateMachine;
    }

    public override void Enter2State()
    {
        _stateMachine.Agent.SetDestination(_cashierPos.position);
    }

    public override void Exit2State()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
    [Inject]private void GetCashierPosition(Storage storage)
    {
        _cashierPos = storage.CashierPosition.transform;
    }
}
