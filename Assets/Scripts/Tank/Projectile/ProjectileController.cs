using BTG.Utilities;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileController
    {
        private ProjectileDataSO m_Data;
        private ProjectileView m_ProjectileView;
        private ProjectilePool m_Pool;
        private CancellationTokenSource m_Cts;

        public Transform Transform => m_ProjectileView.transform;

        public ProjectileController(ProjectileDataSO projectileData, ProjectilePool pool)
        {
            m_Cts = new CancellationTokenSource();
            m_Data = projectileData;
            m_ProjectileView = Object.Instantiate(projectileData.ProjectileViewPrefab);
            m_ProjectileView.SetController(this);
            m_Pool = pool;
        }

        public void OnDisable()
        {
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_ProjectileView.Rigidbody.AddForce(m_ProjectileView.transform.forward * initialSpeed, ForceMode.Impulse);
        }

        public void OnHitDamageable(IDamageable damageable)
        {
            damageable.TakeDamage(m_Data.Damage);
            _ = Explode();
        }

        private async Task Explode()
        {
            m_ProjectileView.PlayExplosionParticle();
            m_ProjectileView.PlayExplosionSound(m_Data.ExplosionSound);

            try
            {
                await Task.Delay((int)(m_ProjectileView.ExplosionDuration * 1000), m_Cts.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            m_Pool.ReturnProjectile(this);
        }
    }
}

