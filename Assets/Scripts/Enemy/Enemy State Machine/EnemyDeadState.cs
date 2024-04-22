using UnityEngine;


namespace BTG.Enemy
{
    public class EnemyDeadState : EnemyBaseState
    {
        public EnemyDeadState(EnemyState state) : base(state)
        {

        }

        public override void Enter()
        {
            Debug.Log("Enemy died!");
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
