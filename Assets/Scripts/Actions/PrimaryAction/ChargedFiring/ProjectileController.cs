using BTG.Utilities;
using System.Threading;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public class ProjectileController : IDestroyable
    {
        private ChargedFiringDataSO m_Data;
        private ProjectileView m_View;
        private ProjectilePool m_Pool;
        private CancellationTokenSource m_Cts;
        public Transform Transform => m_View.transform;

        public ProjectileController(ChargedFiringDataSO projectileData, ProjectilePool pool)
        {
            m_Cts = new CancellationTokenSource();
            m_Data = projectileData;
            m_Pool = pool;
            m_View = Object.Instantiate(projectileData.ViewPrefab, m_Pool.ProjectileContainer);
            m_View.Config(this);
        }

        public void Init(Transform owner)
        {
            m_View.gameObject.SetActive(true);
            m_View.SetOwner(owner);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
        }

        public void Destroy()
        {
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_Cts);
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_View.Rigidbody.AddForce(m_View.transform.forward * initialSpeed, ForceMode.Impulse);
        }

        public void OnHitSomething(Collider other)
        {
            if (other.TryGetComponent(out IDamageableView damageable))
            {
                if (damageable.Owner == m_View.Owner)
                {
                    return;
                }
                damageable.Damage(m_Data.Damage);
            }

            DoExplosionEffect();

            ResetProjectile();
        }

        private void DoExplosionEffect()
        {
            m_Data.ExplosionFactory.CreateExplosion(Transform.position);
        }

        private void ResetProjectile()
        {
            m_View.Rigidbody.velocity = Vector3.zero;
            m_View.Rigidbody.angularVelocity = Vector3.zero;
            m_View.transform.position = Vector3.zero;
            m_View.transform.rotation = Quaternion.identity;

            m_View.gameObject.SetActive(false);
            m_Pool.ReturnProjectile(this);

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }
    }
}

