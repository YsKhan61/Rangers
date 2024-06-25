using BTG.Utilities;
using System;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public class AutoTargetView : MonoBehaviour, IFiringView
    {
        public Transform Owner { get; private set; }

        private AutoTarget m_Controller;

        private float m_Speed;
        private Transform m_Target;

        private bool m_IsLaunched = false;
        private Quaternion m_FinalRotation;

        public void Configure(AutoTarget controller, Transform target, float speed, Transform owner)
        {
            m_Controller = controller;
            m_Target = target;
            m_Speed = speed;
            Owner = owner;
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
            CheckHitForFiringVieww(collision.collider);
            CheckHitForDamageableView(collision.collider);
            m_Controller.CreateExplosion(transform.position);
            Despawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckHitForDamageableView(other);
            m_Controller.CreateExplosion(transform.position);
            Despawn();
        }

        private void CheckHitForFiringVieww(Collider collider)
        {
            if (collider.TryGetComponent(out IFiringView firingView))
            {
                if (firingView.Owner == Owner)
                {
                    return;
                }
            }
        }

        private void CheckHitForDamageableView(Collider collider)
        {
            if (collider.TryGetComponent(out IDamageableView damageableView))
            {
                m_Controller.OnHitDamageable(damageableView);
            }
        }

        private void Despawn()
        {
            // later we will use object pooling
            m_IsLaunched = false;
            GetComponent<NetworkObject>().Despawn();
            Destroy(gameObject);
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