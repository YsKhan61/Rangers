using System;

namespace BTG.Utilities
{
    public abstract class  BaseState<EState> where EState : Enum
    {
        public EState StateKey { get; private set; }
        public EState NextState { get; protected set; }

        public BaseState(EState key)
        {
            StateKey = key;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
        public EState GetNextState() => NextState;
    }
}
