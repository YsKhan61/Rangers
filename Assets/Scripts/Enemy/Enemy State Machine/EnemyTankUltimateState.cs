namespace BTG.Enemy
{
    public class EnemyTankUltimateState : EnemyTankAliveState
    {
        public EnemyTankUltimateState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.ExecuteUltimateAction();
        }

        public override void Exit()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
