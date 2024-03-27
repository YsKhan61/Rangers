using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank.Projectile
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        private ProjectileDataSO m_ProjectileData;
        private Transform m_ProjectileContainer;
        public Transform ProjectileContainer => m_ProjectileContainer;

        public ProjectilePool(ProjectileDataSO projectileData)
        {
            m_ProjectileData = projectileData;
            m_ProjectileContainer = new GameObject("ProjectileContainer of " + projectileData.name).transform;
        }

        public ProjectileController GetProjectile()
        {
            ProjectileController projectile = GetItem();
            projectile.Init();
            return projectile;
        }

        public void ReturnProjectile(ProjectileController projectile)
        {
            ReturnItem(projectile);
        }

        protected override ProjectileController CreateItem() => new ProjectileController(m_ProjectileData, this);
    }
}


