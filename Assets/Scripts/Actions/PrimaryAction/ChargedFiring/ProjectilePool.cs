using BTG.Utilities;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        private ChargedFiringDataSO m_ProjectileData;
        private Transform m_ProjectileContainer;
        public Transform ProjectileContainer => m_ProjectileContainer;

        public ProjectilePool(ChargedFiringDataSO projectileData)
        {
            m_ProjectileData = projectileData;
            m_ProjectileContainer = new GameObject("ProjectileContainer of " + projectileData.name).transform;
        }

        public ProjectileController GetProjectile() => GetItem();


        public void ReturnProjectile(ProjectileController projectile) => ReturnItem(projectile);

        protected override ProjectileController CreateItem() => new ProjectileController(m_ProjectileData, this);
    }
}


