using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayState : BaseState<BuyerStateMachine.BuyerStates>
{
    private BuyerStateMachine _stateMachine;
    public RunAwayState(BuyerStateMachine.BuyerStates state, BuyerStateMachine currentStateMachine) : base(state)
    {
        _stateMachine = currentStateMachine;
    }

    public override void Enter2State()
    {
        _stateMachine.Agent.SetDestination(new Vector3(-2, 0, 35));
    }

    public override void Exit2State()
    {
    }

    public override void UpdateState()
    {
        if (_stateMachine.Agent.remainingDistance < 0.5f)
        {
            _stateMachine.ObjectPool.DropBackToPool(_stateMachine);
            _stateMachine.EventBus.BuyerGoAwaySignal.Invoke();
            Debug.Log("123");
        }
    }
}
