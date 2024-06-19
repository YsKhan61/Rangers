using BTG.Utilities;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    public class ProjectilePool : GenericObjectPool<ProjectileController>
    {
        private ChargedFiringDataSO m_ProjectileData;

        [Inject]
        private IObjectResolver m_Resolver;

        public ProjectilePool(ChargedFiringDataSO projectileData)
        {
            m_ProjectileData = projectileData;
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


