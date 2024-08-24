using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Going2WorkState : BaseState<WorkerStateMachine.WorkerStates>
{
    private WorkerStateMachine _stateMachine;
    public Going2WorkState(WorkerStateMachine.WorkerStates state, WorkerStateMachine stateMachine) : base(state)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter2State()
    {
        _stateMachine.Agent.SetDestination(_stateMachine.WorkPlace.position);
    }

    public override void Exit2State()
    {
    }

    public override void UpdateState()
    {
        if (_stateMachine.Agent.remainingDistance < 0.5f)
        {
            ChangeStateAction(WorkerStateMachine.WorkerStates.WorkingState);
        }
    }
}
