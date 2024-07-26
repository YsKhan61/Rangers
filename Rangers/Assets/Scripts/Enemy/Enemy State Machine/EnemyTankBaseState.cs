using BTG.StateMachine;


namespace BTG.Enemy
{
    /// <summary>
    /// Any state that is related to the enemy tank should inherit from this class.
    /// </summary>
    public abstract class EnemyTankBaseState : IState
    {
        /// <summary>
        /// The owner of the state. It is the state machine that holds this state.
        /// </summary>
        protected EnemyTankStateMachine owner;
        public EnemyTankBaseState(EnemyTankStateMachine owner) => this.owner = owner;

        public abstract void Enter();

        public abstract void Exit();

        public abstract void Update();
    }
}
