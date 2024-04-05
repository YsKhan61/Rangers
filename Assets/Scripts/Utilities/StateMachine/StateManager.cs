using System;
using System.Collections.Generic;

namespace BTG.Utilities
{
    public abstract class StateManager<EState> where EState : Enum
    {
        protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
        protected BaseState<EState> CurrentState;

        protected bool IsTransitioningState = false;

        public void Update()
        {
            EState nextStateKey = CurrentState.GetNextState();

            if (!IsTransitioningState
                && nextStateKey.Equals(CurrentState.StateKey))
            {
                CurrentState.Update();
            }
            else if (!IsTransitioningState)
            {
                ChangeState(nextStateKey);
            }
        }

        public void ChangeState(EState newState)
        {
            IsTransitioningState = true;

            CurrentState?.Exit();

            CurrentState = States[newState];
            CurrentState.Enter();

            IsTransitioningState = false;
        }
    }
}
