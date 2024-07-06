using BTG.Utilities;
using System.Threading;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public class AutoTargetView : MonoBehaviour, IFiringView
    {
        public Transform Owner { get; private set; }

        protected AutoTarget m_Controller;

        private float m_Speed;
        private Transform m_Target;

        protected bool isLaunched = false;
        private Quaternion m_FinalRotation;
        private CancellationTokenSource m_Cts;

        public void Configure(AutoTarget controller, Transform target, float speed, Transform owner)
        {
            m_Controller = controller;
            m_Target = target;
            m_Speed = speed;
            Owner = owner;
            isLaunched = false;
        }

        public void Launch()
        {
            if (m_Target == null)
            {
                Debug.LogError("Target position is not set");
                return;
            }

            isLaunched = true;
        }

        public void AutoDestroy(int delay)
        {
            m_Cts = new CancellationTokenSource();
            _ = HelperMethods.InvokeAfterAsync(delay, () =>Despawn(), m_Cts.Token);
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
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
            Despawn();
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckHitForDamageableView(other);
            m_Controller.CreateExplosion(transform.position);
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
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

        protected virtual void Despawn()
        {
            // later we will use object pooling
            isLaunched = false;
            Destroy(gameObject);
        }

        private void UpdateProjectilePosition()
        {
            if (!isLaunched)
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