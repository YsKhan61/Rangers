using UnityEngine;

namespace BTG.Enemy
{
    /// <summary>
    /// The state of the enemy tank when it is executing its Invisibility ultimate action.
    /// </summary>
    public class EnemyTankInvisibilityState : EnemyTankUltimateState
    {
        private const float TELEPORT_RANGE = 20f;

        public EnemyTankInvisibilityState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Teleport();
        }

        private void Teleport()
        {
            Vector3 randomPosition = GetRandomPositionInNavMesh();
            owner.Agent.Warp(randomPosition);
        }

        private Vector3 GetRandomPositionInNavMesh()
        {
            Vector3 randomPosition = Vector3.zero;

            while (randomPosition != Vector3.zero)
            {
                Vector2 randomPoint = Random.insideUnitCircle * TELEPORT_RANGE;
                randomPosition = owner.Transform.position + new Vector3(randomPoint.x, owner.Transform.position.y, randomPoint.y);
                UnityEngine.AI.NavMeshHit hit;
                if (UnityEngine.AI.NavMesh.SamplePosition(randomPosition, out hit, TELEPORT_RANGE, UnityEngine.AI.NavMesh.AllAreas))
                {
                    randomPosition = hit.position;
                }
            }
            

            return randomPosition;
        }
    }
}
