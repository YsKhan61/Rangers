using UnityEngine;


namespace BTG.Enemy
{
    /// <summary>
    /// This state is for the enemy tank when it is dead.
    /// </summary>
    public class EnemyTankDeadState : EnemyTankBaseState
    {
        public EnemyTankDeadState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enemy died!");
        }

        public override void Exit()
        {

        }

        public override void Update()
        {

        }
    }
}
