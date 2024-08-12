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
        //_stateMachine.Animator.SetBool(_stateMachine.waiting, false);
        _stateMachine.CurrentItemInList = ItemName.Cashier;
        //_stateMachine.Agent.SetDestination(_stateMachine.Storage.GetPosition(ItemName.Cashier));
        _stateMachine.Agent.SetDestination(_stateMachine.GetWannaBuyObjPosition(ItemName.Cashier));
        _stateMachine.QueueHandler.AddBuyerToQueue(_stateMachine, _stateMachine.CurrentItemInList,
            _stateMachine.Storage.IShowcaseDictionary[_stateMachine.CurrentItemInList]);

        //_stateMachine.Storage.IShowcaseDictionary[ItemName.Cashier].Subscribe2BuyerGoingSignal(_stateMachine.MovingForwardInQueue, ref _stateMachine.NumerationOfBuyerInQueue);
    }

    public override void Exit2State()
    {
        //_stateMachine.Storage.IShowcaseDictionary[ItemName.Cashier].Unsubscribe2BuyerGoingSignal(_stateMachine.MovingForwardInQueue);
        _stateMachine.QueueHandler.RemoveBuyerFromQueue(_stateMachine, _stateMachine.CurrentItemInList, 
            _stateMachine.Storage.IShowcaseDictionary[_stateMachine.CurrentItemInList]);

    }

    public override void UpdateState()
    {
    }
}
