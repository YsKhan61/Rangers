using UnityEngine;

namespace BTG.Enemy
{
    public class EnemyMoveState : EnemyAliveState
    {
        private int m_LastIndex;

        public EnemyMoveState(EnemyStateManager.EnemyState state) : base(state)
        {

        }

        public override void Enter()
        {
            NextState = EnemyStateManager.EnemyState.Move;
            SetNewDestination();
        }

        public override void Update()
        {
            // check if enemy is near destination, if yes, set new destination
            if (HasReachedDestination())
                NextState = EnemyStateManager.EnemyState.Idle;
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
