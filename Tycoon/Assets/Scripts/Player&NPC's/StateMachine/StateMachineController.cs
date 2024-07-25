using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineController<TState> : MonoBehaviour where TState : Enum
{
    protected Dictionary<TState, BaseState<TState>> States = new Dictionary<TState, BaseState<TState>>();

    protected BaseState<TState> CurrentState;

    protected bool IsOnTransition;

    protected void StartMachine(TState StartingState)
    {
        if (CurrentState != null)
        {
            CurrentState.ChangeStateAction -= Transition2nextState;
        }
        CurrentState = States[StartingState];
        CurrentState.ChangeStateAction += Transition2nextState;
        CurrentState.Enter2State();
    }

    private void Update()
    {
        if(!IsOnTransition)
        {
            CurrentState.UpdateState();
        }
    }

    private void Transition2nextState(TState state)
    {
        IsOnTransition = true;
        CurrentState.ChangeStateAction -= Transition2nextState;
        CurrentState.Exit2State();
        CurrentState = States[state];
        CurrentState.ChangeStateAction += Transition2nextState;
        CurrentState.Enter2State();
        IsOnTransition = false;
    }
    
    protected void ChangeActionFromChildren(TState tState)
    {
        CurrentState.ChangeStateAction.Invoke(tState);
    }
}
