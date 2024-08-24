using System;
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
        if (_stateMachine.WorkerWork == Works.TransporterWorker)
            _stateMachine.StartCoroutine(CollectStuffFromFabrics());
        else if (_stateMachine.WorkerWork == Works.CashierWorker)
            return;
        _stateMachine.StartCoroutine(WorkingTime());
    }

    public override void Exit2State()
    {
        
    }

    public override void UpdateState()
    {

    }

    private IEnumerator WorkingTime() 
    {
        yield return new WaitForSeconds(_stateMachine.WorkerStruct.WorkingTime);
        yield return new WaitUntil(() => _stateMachine.BackpackWorker.Items.Count > 0);
        _stateMachine.StartCoroutine(Rest());
    }
    private IEnumerator CollectStuffFromFabrics()
    {
        for (int index = 0; index < _stateMachine.WorkerStruct.Fabrics.Count; index++)
        {
            Debug.Log(index);
            _stateMachine.Agent.SetDestination(_stateMachine.WorkerStruct.Fabrics[index].transform.position);
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => _stateMachine.Agent.remainingDistance < 0.1f);
            if (_stateMachine.WorkerStruct.Fabrics[index].CurrentGrowSecond != _stateMachine.WorkerStruct.Fabrics[index].GrowSpeed)
                yield return new WaitForSeconds(_stateMachine.WorkerStruct.Fabrics[index].GrowSpeed - _stateMachine.WorkerStruct.Fabrics[index].CurrentGrowSecond);
            else
                Debug.Log("qweqwerqwe");
        }
        if (_stateMachine.BackpackWorker.Items.Count > 0)
        {
            _stateMachine.StartCoroutine(GiveItems2Showcase());
        }
        else
            _stateMachine.StartCoroutine(CollectStuffFromFabrics());

    }

    private IEnumerator GiveItems2Showcase()
    {
        _stateMachine.Agent.SetDestination(_stateMachine.WorkerStruct.ShowcaseTransform.position);
        yield return new WaitForSeconds(1f);
        Debug.Log(_stateMachine.Agent.remainingDistance);
        yield return new WaitWhile(() => _stateMachine.Agent.remainingDistance > 0.1f);
        Debug.Log(_stateMachine.Agent.remainingDistance);
        yield return new WaitForSeconds(1f);
        _stateMachine.StartCoroutine(CollectStuffFromFabrics());
    }

    private IEnumerator Rest()
    {
        _stateMachine.Agent.SetDestination(_stateMachine.WorkerStruct.ShowcaseTransform.position);
        yield return new WaitUntil(() => _stateMachine.Agent.remainingDistance < 0.1f);
    }

}
