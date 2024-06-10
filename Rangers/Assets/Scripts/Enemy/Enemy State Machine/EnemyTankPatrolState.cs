using UnityEngine;

namespace BTG.Enemy
{
    public class EnemyTankPatrolState : EnemyTankAliveState
    {
        private int m_LastIndex;

        public EnemyTankPatrolState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            SetNewDestination();
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            // check if enemy is near destination, if yes, notify the state machine
            if (HasReachedDestination())
                owner.OnPatrolStateComplete();
        }

        private void SetNewDestination()
        {
            int newIndex = m_LastIndex;
            while (newIndex == m_LastIndex)
            {
                newIndex = Random.Range(0, owner.PatrolPoints.Length);
            }
            m_LastIndex = newIndex;
            owner.Agent.SetDestination(owner.PatrolPoints[newIndex]);
        }

        private bool HasReachedDestination()
        {
            return !owner.Agent.pathPending
                && owner.Agent.remainingDistance <= owner.Agent.stoppingDistance
                && (!owner.Agent.hasPath || owner.Agent.velocity.sqrMagnitude == 0f);
        }
    }
}
