using BTG.Utilities;


namespace BTG.Enemy
{
    /// <summary>
    /// This state is for the enemy when it is alive,
    /// Various alive states such as Idle, Move, Attack, etc. will inherit from this state.
    /// </summary>
    public abstract class EnemyAliveState : EnemyBaseState
    {
        public EnemyAliveState(EnemyStateManager.EnemyState state) : base(state)
        {

        }
    }
}
