using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingState : BaseState<WorkerStateMachine.WorkerStates>
{
    private WorkerStateMachine _stateMachine;
    public WorkingState(WorkerStateMachine.WorkerStates state, WorkerStateMachine stateMachine) : base(state)
    {
        _stateMachine = stateMachine;
    }

    public override void Enter2State()
    {

    }

    public override void Exit2State()
    {

    }

    public override void UpdateState()
    {

    }
}
