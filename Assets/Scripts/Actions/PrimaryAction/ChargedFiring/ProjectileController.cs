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
            m_View.SetController(this);
        }

        public void Init()
        {
            m_View.gameObject.SetActive(true);
            UnityCallbacks.Instance.Register(this);
        }

        public void OnDestroy()
        {
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_View.Rigidbody.AddForce(m_View.transform.forward * initialSpeed, ForceMode.Impulse);
        }

        public void OnHitDamageable(IDamageable damageable)
        {
            damageable.TakeDamage(m_Data.Damage);
            Explode();
        }

        public void ResetProjectile()
        {
            m_View.Rigidbody.velocity = Vector3.zero;
            m_View.Rigidbody.angularVelocity = Vector3.zero;
            m_View.transform.position = Vector3.zero;
            m_View.transform.rotation = Quaternion.identity;
            m_View.gameObject.SetActive(false);
            m_Pool.ReturnProjectile(this);

            UnityCallbacks.Instance.Unregister(this);
        }

        private void Explode()
        {
            m_View.PlayExplosionParticle();
            m_View.PlayExplosionSound(m_Data.ExplosionSound);

            _ = HelperMethods.InvokeAfterAsync(((int)m_View.ExplosionDuration), () => ResetProjectile(), m_Cts.Token);
        }
    }
}

