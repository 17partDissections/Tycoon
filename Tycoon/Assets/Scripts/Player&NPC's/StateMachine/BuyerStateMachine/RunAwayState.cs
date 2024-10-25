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
        _stateMachine.Agent.SetDestination(new Vector3(0, 0, 43));
    }

    public override void Exit2State()
    {
        _stateMachine.BackpackBuyer.DropBack2PoolAllItems();
    }

    public override void UpdateState()
    {
        if (_stateMachine.Agent.remainingDistance < 0.5f)
        {
            _stateMachine.gameObject.SetActive(false);
            _stateMachine.EventBus.BuyerGoAwaySignal.Invoke();
        }
    }
}
