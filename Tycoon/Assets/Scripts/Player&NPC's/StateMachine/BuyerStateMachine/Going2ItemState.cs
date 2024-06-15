

public class Going2ItemState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    public Going2ItemState(BuyerStateMachine.BuyerStates state,
        BuyerStateMachine CurrentStateMachine) : base(state)
    {
        _stateMachine = CurrentStateMachine;
    }

    public override void Enter2State()
    {
        _stateMachine.Agent.SetDestination(_stateMachine.GetWannaBuyObjPosition(_stateMachine.WannaBuy[0]).position);
    }

    public override void Exit2State()
    {

    }

    public override void UpdateState()
    {

    }


}
