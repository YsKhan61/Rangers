using UnityEngine;

namespace BTG.Enemy
{
    /// <summary>
    /// This is a special state for the enemy tank when it is damaged.
    /// It is used to disable the agent and make the tank immobile for a certain duration.
    /// This helps the EnemyTankController's transform to stay alligned with the damageable collider's transform
    /// </summary>
    public class EnemyTankDamagedState : EnemyTankAliveState
    {
        private const int DURATION = 1;
        private float m_TimeElapsed;

        public EnemyTankDamagedState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            m_TimeElapsed = 0;
            owner.Agent.enabled = false;
        }

        public override void Exit()
        {
            owner.Agent.enabled = true;
            owner.ReAllign();
        }

        public override void Update()
        {
            if (m_TimeElapsed >= DURATION)
            {
                owner.OnDamagedStateComplete();
                return;
            }

            m_TimeElapsed += Time.deltaTime;
        }
    }
}
