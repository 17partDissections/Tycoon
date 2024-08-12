using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class StartState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    public StartState(BuyerStateMachine.BuyerStates state,
        BuyerStateMachine CurrentStateMachine) : base(state)
    {
        _stateMachine = CurrentStateMachine;
    }

    public override void Enter2State()
    {
        //_stateMachine.Animator.SetBool(_stateMachine.waiting, false);
        _stateMachine.Agent.SetDestination(_stateMachine.WayPoint);
    }

    public override void Exit2State()
    {

    }

    public override void UpdateState()
    {
        if (_stateMachine.Agent.remainingDistance <0.5f)
        {
            ChangeStateAction(BuyerStateMachine.BuyerStates.ThinkinState);
        }

    }

    
}
