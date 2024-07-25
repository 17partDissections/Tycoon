using UnityEngine;

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
        _stateMachine.Animator.SetBool(_stateMachine.waiting, false);
        _stateMachine.CurrentItemInList = _stateMachine.WannaBuy[0];
        _stateMachine.Agent.SetDestination(_stateMachine.GetWannaBuyObjPosition(_stateMachine.WannaBuy[0]));
        _stateMachine.Storage.ChangePositionInQueue(_stateMachine.WannaBuy[0], true);
        _stateMachine.Storage.IShowcaseDictionary[_stateMachine.WannaBuy[0]].Subscribe2BuyerGoingSignal(_stateMachine.MovingForwardInQueue, ref _stateMachine.NumerationOfBuyerInQueue);
        Debug.Log(_stateMachine.gameObject.name +"   "+ _stateMachine.NumerationOfBuyerInQueue);
    }

    public override void Exit2State()
    {
        _stateMachine.Storage.IShowcaseDictionary[_stateMachine.WannaBuy[0]].Unsubscribe2BuyerGoingSignal(_stateMachine.MovingForwardInQueue);
    }

    public override void UpdateState()
    {
    }
    public void Subscribe2NewItem(ItemName itemName)
    {
        _stateMachine.Storage.IShowcaseDictionary[_stateMachine.CurrentItemInList].Unsubscribe2BuyerGoingSignal(_stateMachine.MovingForwardInQueue);
        _stateMachine.CurrentItemInList = itemName;
        _stateMachine.Storage.IShowcaseDictionary[_stateMachine.CurrentItemInList].Subscribe2BuyerGoingSignal(_stateMachine.MovingForwardInQueue, ref _stateMachine.NumerationOfBuyerInQueue);
    }





}
