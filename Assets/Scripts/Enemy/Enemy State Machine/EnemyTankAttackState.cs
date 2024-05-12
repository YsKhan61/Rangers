using UnityEngine;

namespace BTG.Enemy
{
    public class EnemyTankAttackState : EnemyTankBaseState
    {
        private const float ROTATE_THRESHOLD = 0.1f;
        private const float ROTATE_SPEED = 5f;
        private const float SHOOT_COOLDOWN = 2f;

        private float m_ShootTimer;
        private Transform m_TargetTransform;

        public EnemyTankAttackState(EnemyTankStateMachine owner) : base(owner)
        {
        }

        public override void Enter()
        {
            m_ShootTimer = 0f;
            m_TargetTransform = owner.TargetTransform;
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

            if (HasRotatedTowardsTarget())
            {
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
            if (m_ShootTimer >= SHOOT_COOLDOWN)
            {
                m_ShootTimer = 0f;
                owner.ExecutePrimaryAction();
            }
        }
    }
}
