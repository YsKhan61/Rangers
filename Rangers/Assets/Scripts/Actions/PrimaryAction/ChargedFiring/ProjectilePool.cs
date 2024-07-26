using BTG.Utilities;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    public class ProjectilePool : GenericObjectPool<ProjectileView>
    {
        private ProjectileView m_Prefab;

        public ProjectilePool(ProjectileView prefab) => m_Prefab = prefab;

        public ProjectileView GetProjectile() => GetItem();

        public void ReturnProjectile(ProjectileView projectile) => ReturnItem(projectile);

        protected override ProjectileView CreateItem()
        {
            ProjectileView view = Object.Instantiate(m_Prefab);
            view.SetPool(this);
            return view;
        }
    }
}


