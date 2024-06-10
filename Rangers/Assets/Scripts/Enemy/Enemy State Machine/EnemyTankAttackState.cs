using UnityEngine;

namespace BTG.Enemy
{
    public class EnemyTankAttackState : EnemyTankAliveState
    {
        private const float ROTATE_THRESHOLD = 1f;
        private const float ROTATE_SPEED = 2f;

        private float m_ShootTimer;
        private float m_ShootCooldown;
        private Transform m_TargetTransform;

        public EnemyTankAttackState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            if (owner.TargetTransform == null)
            {
                owner.OnTargetNotInRange();
                return;
            }
            m_TargetTransform = owner.TargetTransform;

            m_ShootTimer = 0f;
            m_ShootCooldown = 0f;
            
            owner.Agent.enabled = false;
        }

        public override void Exit()
        {
            m_TargetTransform = null;
            owner.Agent.enabled = true;
        }

        public override void Update()
        {
            if (!owner.IsTargetInRange)
            {
                owner.OnTargetNotInRange();
                return;
            }

            if (HasRotatedTowardsTarget() && !owner.IsPrimaryActionExecuting)
            {
                if (owner.IsUltimateReady)
                {
                    owner.OnUltimateReady();
                    return;
                }

                TryShoot();
                return;
            }

            RotateTowardsTarget();
        }

        private void RotateTowardsTarget()
        {
            Vector3 direction = (m_TargetTransform.position - owner.Transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            owner.Transform.rotation = Quaternion.Slerp(owner.Transform.rotation, lookRotation, Time.deltaTime * ROTATE_SPEED);
        }

        private bool HasRotatedTowardsTarget()
        {
            Vector3 direction = (m_TargetTransform.position - owner.Transform.position).normalized;
            float angle = Vector3.Angle(owner.Transform.forward, direction);
            return angle < ROTATE_THRESHOLD;
        }

        private void TryShoot()
        {
            m_ShootTimer += Time.deltaTime;
            if (m_ShootTimer >= m_ShootCooldown)
            {
                m_ShootTimer = 0f;
                m_ShootCooldown = Random.Range(1, 3); 
                owner.ExecutePrimaryAction((int)m_ShootCooldown);
            }
        }
    }
}
