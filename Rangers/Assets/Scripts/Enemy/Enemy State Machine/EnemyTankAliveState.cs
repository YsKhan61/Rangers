namespace BTG.Enemy
{
    /// <summary>
    /// This abstract state is for the enemy tank when it is alive.
    /// Any alive state of the enemy tank should inherit from this class.
    /// </summary>
    public abstract class EnemyTankAliveState : EnemyTankBaseState
    {
        protected EnemyTankAliveState(EnemyTankStateMachine owner) : base(owner)
        {
        }
    }
}
