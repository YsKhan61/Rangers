using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class ProjectileController
    {
        private ProjectileModel m_ProjectileModel;
        private ProjectileView m_ProjectileView;

        public Transform Transform => m_ProjectileView.transform;

        public ProjectileController(ProjectileDataSO projectileData)
        {
            m_ProjectileModel = new ProjectileModel(projectileData, this);
            m_ProjectileView = Object.Instantiate(projectileData.ProjectileViewPrefab);
            m_ProjectileView.SetController(this);
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_ProjectileView.Rigidbody.AddForce(m_ProjectileView.transform.forward * initialSpeed, ForceMode.Impulse);
        }
    }
}

