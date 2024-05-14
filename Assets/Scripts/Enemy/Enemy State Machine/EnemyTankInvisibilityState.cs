namespace BTG.Enemy
{
    /// <summary>
    /// The state of the enemy tank when it is executing its Invisibility ultimate action.
    /// </summary>
    public class EnemyTankInvisibilityState : EnemyTankUltimateState
    {
        public EnemyTankInvisibilityState(EnemyTankStateMachine owner) : base(owner)
        {
        }
    }
}
