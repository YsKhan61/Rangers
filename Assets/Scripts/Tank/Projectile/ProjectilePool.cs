using BTG.Utilities;

namespace BTG.Tank.Projectile
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        private ProjectileDataSO m_ProjectileData;

        public ProjectilePool(ProjectileDataSO projectileData)
        {
            m_ProjectileData = projectileData;
        }

        public ProjectileController GetProjectile()
        {
            return GetItem();
        }

        protected override ProjectileController CreateItem() => new ProjectileController(m_ProjectileData);
    }
}


