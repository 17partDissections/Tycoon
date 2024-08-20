using UnityEngine;
using UnityEngine.AI;

public class WorkerStateMachine : StateMachineController<WorkerStateMachine.WorkerStates>
{
    [field:SerializeField] public Transform WorkPlace { get; private set; }
    public NavMeshAgent Agent;
    public BackpackWorker BackpackWorker;
    [field: SerializeField] public Works WorkerWork { get; private set; }   

    public enum WorkerStates 
    {
        Going2WorkState,
        WorkingState
    }

    private void Awake()
    {
        Going2WorkState Going2Work = new Going2WorkState(WorkerStates.Going2WorkState, this);
        States.Add(WorkerStates.Going2WorkState, Going2Work);
        WorkingState WorkingState = new WorkingState(WorkerStates.WorkingState, this);
        States.Add(WorkerStates.WorkingState, WorkingState);
        StartMachine(WorkerStates.Going2WorkState);
    }
}
public enum Works
{
    CashierWorker
}
