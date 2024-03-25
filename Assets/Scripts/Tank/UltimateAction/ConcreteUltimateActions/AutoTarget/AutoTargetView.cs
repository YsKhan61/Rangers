using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank.UltimateAction
{

    public class AutoTargetView : MonoBehaviour
    {
        private float m_Speed;
        private Transform m_Target;

        private bool m_IsLaunched = false;
        private float m_AngleToTarget;
        private Quaternion m_FinalRotation;

        public void Configure(Transform target, float m_Speed)
        {
            m_Target = target;
            this.m_Speed = m_Speed;

            m_IsLaunched = false;
        }

        public void Launch()
        {
            if (m_Target == null)
            {
                Debug.LogError("Target position is not set");
                return;
            }

            m_IsLaunched = true;
        }

        private void Update()
        {
            UpdateProjectilePosition();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == m_Target.gameObject)
            {
                Debug.Log("Auto target projectile hit the target!");
            }
            else
            {
                Debug.Log("Auto target projectile hit something else!");
            }

            m_IsLaunched = false;
        }

        private void UpdateProjectilePosition()
        {
            if (!m_IsLaunched)
            {
                return;
            }

            m_FinalRotation = Quaternion.FromToRotation(
                transform.forward, 
                (m_Target.position.SetYOffset(0.5f) - transform.position).normalized) * 
                transform.rotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, m_FinalRotation, m_Speed * Time.deltaTime);
            transform.position += m_Speed * Time.deltaTime * transform.forward;
        }
    }
}