using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileController
    {
        private ProjectileView m_ProjectileView;

        public Transform Transform => m_ProjectileView.transform;

        public ProjectileController(ProjectileDataSO projectileData)
        {
            m_ProjectileView = Object.Instantiate(projectileData.ProjectileViewPrefab);
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_ProjectileView.Rigidbody.AddForce(m_ProjectileView.transform.forward * initialSpeed, ForceMode.Impulse);
        }
    }
}

