namespace BTG.Enemy
{
    public class  EnemyTankAutoTargetState : EnemyTankUltimateState
    {
        public EnemyTankAutoTargetState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.ExecuteUltimateAction();
        }
    }
}
