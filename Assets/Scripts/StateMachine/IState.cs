namespace BTG.StateMachine
{
    /// <summary>
    /// An interface that represents a state.
    /// Any state of the project should implement this interface.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Enter the state.
        /// </summary>
        public void Enter();

        /// <summary>
        /// Update the state.
        /// </summary>
        public void Update();

        /// <summary>
        /// Exit the state.
        /// </summary>
        public void Exit();
    }
}