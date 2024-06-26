using BTG.Utilities;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    public class NetworkProjectilePool : GenericObjectPool<NetworkProjectileView>
    {
        private NetworkProjectileView m_Prefab;

        [Inject]
        private IObjectResolver m_Resolver;

        public NetworkProjectilePool(NetworkProjectileView prefab) => m_Prefab = prefab;

        public NetworkProjectileView GetProjectile() => GetItem();

        public void ReturnProjectile(NetworkProjectileView projectile) => ReturnItem(projectile);

        protected override NetworkProjectileView CreateItem()
        {
            NetworkProjectileView view = Object.Instantiate(m_Prefab);
            view.NetworkObject.Spawn(true);
            view.SetPool(this);
            return view;
        }
    }
}


