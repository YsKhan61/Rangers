﻿namespace BTG.Enemy
{
    public class EnemyTankAirStrikeState : EnemyTankUltimateState
    {
        public EnemyTankAirStrikeState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            owner.ExecuteUltimateAction();
        }
    }
}
