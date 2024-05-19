using System;
using System.Collections.Generic;

namespace BTG.StateMachine
{
    /// <summary>
    /// An abstract class that represents a state machine.
    /// Any state machine of the project should inherit from this class.
    /// </summary>
    public abstract class BaseStateMachine<T> where T : Enum
    {
        internal protected event Action<T> OnStateChanged;

        protected Dictionary<T, IState> m_States = new Dictionary<T, IState>();

        /// <summary>
        /// The current state of the state machine.
        /// </summary>
        protected IState currentState { get; private set; }

        /// <summary>
        /// Changes the current state of the state machine.
        /// It will be only called by the state machine itself.
        /// </summary>
        /// <param T="state">the enum of the  state to transition to</param>
        protected void ChangeState(T state)
        {
            if (!m_States.TryGetValue(state, out IState nextState))
                throw new ArgumentException($"State {state} does not exist in the state machine.");

            currentState?.Exit();
            currentState = nextState;
            OnStateChanged?.Invoke(state);
            currentState.Enter();
        }

        /// <summary>
        /// Add a new state to the state machine.
        /// </summary>
        /// <param name="state">the enum type of the state to add</param>
        /// <param name="stateInstance">the instance of the state to add</param>
        /// <exception cref="ArgumentException">if the enum type is already present in the dictionary, throw an exception</exception>
        protected void AddState(T state, IState stateInstance)
        {
            if (m_States.ContainsKey(state))
                throw new ArgumentException($"State {state} already exists in the state machine.");

            m_States.Add(state, stateInstance);
        }
    }
}