using BTG.Utilities;

namespace BTG.Tank.Projectile
{
    public class TankProjectilePool : GenericObjectPool<TankProjectileController>
    {
        private TankProjectileDataSO m_ProjectileData;

        public TankProjectilePool(TankProjectileDataSO projectileData)
        {
            m_ProjectileData = projectileData;
        }

        public TankProjectileController GetProjectile()
        {
            return GetItem();
        }

        protected override TankProjectileController CreateItem() => new TankProjectileController(m_ProjectileData);
    }
}


