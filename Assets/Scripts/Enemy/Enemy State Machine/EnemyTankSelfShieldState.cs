namespace BTG.Enemy
{
    public class EnemyTankSelfShieldState : EnemyTankUltimateState
    {
        public EnemyTankSelfShieldState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.ExecuteUltimateAction();
        }
    }
}
