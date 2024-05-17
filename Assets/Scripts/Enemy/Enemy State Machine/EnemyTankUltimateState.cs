namespace BTG.Enemy
{
    /// <summary>
    /// This state is responsible for the ultimate action of the enemy tank.
    /// Enemies can have different ultimate actions.
    /// Subclasses of this state can be created for different ultimate actions.
    /// </summary>
    public abstract class EnemyTankUltimateState : EnemyTankAliveState
    {
        public EnemyTankUltimateState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
