using BTG.Utilities;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        private ChargedFiringDataSO m_ProjectileData;
        private Transform m_ProjectileContainer;
        public Transform ProjectileContainer => m_ProjectileContainer;

        [Inject]
        private IObjectResolver m_Resolver;

        public ProjectilePool(ChargedFiringDataSO projectileData)
        {
            m_ProjectileData = projectileData;
            m_ProjectileContainer = new GameObject("ProjectileContainer").transform;
        }

        public ProjectileController GetProjectile() => GetItem();


        public void ReturnProjectile(ProjectileController projectile) => ReturnItem(projectile);

        protected override ProjectileController CreateItem()
        {
            ProjectileController pc = new (m_ProjectileData, this);
            m_Resolver.Inject(pc);
            return pc;
        }
    }
}


