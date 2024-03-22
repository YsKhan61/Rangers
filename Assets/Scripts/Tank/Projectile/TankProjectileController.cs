using UnityEngine;


namespace BTG.Tank.Projectile
{
    public class TankProjectileController
    {
        private TankProjectileModel m_ProjectileModel;
        private TankProjectileView m_ProjectileView;

        public Transform Transform => m_ProjectileView.transform;

        public TankProjectileController(TankProjectileDataSO projectileData)
        {
            m_ProjectileModel = new TankProjectileModel(projectileData, this);
            m_ProjectileView = Object.Instantiate(projectileData.ProjectileViewPrefab);
            m_ProjectileView.SetController(this);
        }

        public void AddImpulseForce(float initialSpeed)
        {
            m_ProjectileView.Rigidbody.AddForce(m_ProjectileView.transform.forward * initialSpeed, ForceMode.Impulse);
        }
    }
}

