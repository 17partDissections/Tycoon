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
        //_animator.SetBool(_stateMachine.Walking, true);
        _stateMachine.Agent.SetDestination(_stateMachine.WayPoint.transform.position);
    }

    public override void Exit2State()
    {

    }

    public override void UpdateState()
    {
        if (_stateMachine.Agent.remainingDistance <0.1f)
        {
            ChangeStateAction(BuyerStateMachine.BuyerStates.ThinkinState);
        }

    }

    
}
