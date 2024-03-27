using BTG.Utilities;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileController
    {
        private ProjectileDataSO m_Data;
        private ProjectileView m_View;
        private ProjectilePool m_Pool;
        private CancellationTokenSource m_Cts;

        public Transform Transform => m_View.transform;

        public ProjectileController(ProjectileDataSO projectileData, ProjectilePool pool)
        {
            m_Cts = new CancellationTokenSource();
            m_Data = projectileData;
            m_Pool = pool;
            m_View = Object.Instantiate(projectileData.ProjectileViewPrefab, m_Pool.ProjectileContainer);
            m_View.SetController(this);
        }

        public void Init()
        {
            m_View.gameObject.SetActive(true);
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
            _ = Explode();
        }

        public void ResetProjectile()
        {
             m_View.Rigidbody.velocity = Vector3.zero;
            m_View.Rigidbody.angularVelocity = Vector3.zero;
            m_View.transform.position = Vector3.zero;
            m_View.transform.rotation = Quaternion.identity;
            m_View.gameObject.SetActive(false);
            m_Pool.ReturnProjectile(this);
        }

        private async Task Explode()
        {
            m_View.PlayExplosionParticle();
            m_View.PlayExplosionSound(m_Data.ExplosionSound);

            try
            {
                await Task.Delay((int)(m_View.ExplosionDuration * 1000), m_Cts.Token);
                ResetProjectile();
            }
            catch (TaskCanceledException)
            {

            }
        }
    }
}

