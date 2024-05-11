using UnityEngine;

namespace BTG.Enemy
{
    public class EnemyPatrolState : EnemyAliveState
    {
        private int m_LastIndex;

        public EnemyPatrolState(EnemyState state) : base(state)
        {

        }

        public override void Enter()
        {
            NextState = EnemyState.Move;
            SetNewDestination();
        }

        public override void Update()
        {
            if (m_Controller.IsTargetInRange)
            {
                // if target is in range, switch to chase state
            }

            // check if enemy is near destination, if yes, set new destination
            if (HasReachedDestination())
                NextState = EnemyState.Idle;
        }

        public override void Exit()
        {
            // do nothing for now
        }

        private void SetNewDestination()
        {
            int newIndex = m_LastIndex;
            while (newIndex == m_LastIndex)
            {
                newIndex = Random.Range(0, m_Controller.Data.PatrolPoints.Length);
            }
            m_LastIndex = newIndex;

            m_Controller.Agent.SetDestination(m_Controller.Data.PatrolPoints[newIndex]);
        }

        private bool HasReachedDestination()
        {
            return !m_Controller.Agent.pathPending
                && m_Controller.Agent.remainingDistance <= m_Controller.Agent.stoppingDistance
                && (!m_Controller.Agent.hasPath || m_Controller.Agent.velocity.sqrMagnitude == 0f);
        }
    }
}
