using UnityEngine;

namespace BTG.Enemy
{
    /// <summary>
    /// The enemy tank idle state.
    /// </summary>
    public class EnemyTankIdleState : EnemyTankAliveState
    {
        private const int MAX_IDLE_TIME = 3;
        private float m_IdleTime = 0;
        private float m_TimeElapsed = 0;

        public EnemyTankIdleState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            m_TimeElapsed = 0;
            m_IdleTime = Random.Range(0, MAX_IDLE_TIME);
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            m_TimeElapsed += Time.deltaTime;
            if (m_TimeElapsed >= m_IdleTime)
                owner.OnIdleStateComplete();
        }
    }
}
