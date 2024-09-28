using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WorkerStateMachine : StateMachineController<WorkerStateMachine.WorkerStates>
{
    [field:SerializeField] public Transform WorkPlace { get; private set; }
    public NavMeshAgent Agent;
    [SerializeField] private Animator _animator;
    public BackpackWorker BackpackWorker;
    [SerializeField] public WorkerStruct WorkerStruct;
    [field: SerializeField] public Works WorkerWork { get; private set; }   

    public enum WorkerStates 
    {
        Going2WorkState,
        WorkingState
    }

    [HideInInspector] public int Walking;

    private void Awake()
    {
        WorkerStruct.SpawnTransform = transform;
        Walking = Animator.StringToHash("Walking");

        Going2WorkState Going2Work = new Going2WorkState(WorkerStates.Going2WorkState, this);
        States.Add(WorkerStates.Going2WorkState, Going2Work);
        WorkingState WorkingState = new WorkingState(WorkerStates.WorkingState, this);
        States.Add(WorkerStates.WorkingState, WorkingState);
        StartMachine(WorkerStates.Going2WorkState);
        CheckAgentVelocityAsync().Forget();
    }
    private async UniTask CheckAgentVelocityAsync()
    {
        while (true)
        {
            await UniTask.WaitUntilValueChanged(this, x => x.Agent.velocity.magnitude > 0.1f);
            _animator.SetBool(Walking, false);
            await UniTask.WaitWhile(() => Agent.velocity.magnitude > 0);
            _animator.SetBool(Walking, true);
        }


    }
}

public enum Works
{
    CashierWorker,
    TransporterWorker
}
