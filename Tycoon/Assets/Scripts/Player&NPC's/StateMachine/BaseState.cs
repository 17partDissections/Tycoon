using System;

public abstract class BaseState<TState> where TState : Enum
{
    public TState StateName;
    public Action <TState> ChangeStateAction;
    protected bool IsOnTransition;

    public BaseState(TState state)
    {
        StateName = state;
    }

    public abstract void Enter2State();
    public abstract void UpdateState();
    public abstract void Exit2State();




}
