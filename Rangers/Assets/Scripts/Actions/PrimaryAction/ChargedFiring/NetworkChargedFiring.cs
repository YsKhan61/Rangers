using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;


namespace BTG.Actions.PrimaryAction
{
    public class NetworkChargedFiring : ChargedFiringBase
    {
        private NetworkProjectilePool m_Pool;

        public NetworkChargedFiring(ChargedFiringDataSO data, NetworkProjectilePool projectilePool) : base(data)
        {
            m_Pool = projectilePool;
        }

        public override void Destroy()
        {
            m_Pool.ClearPool();
            base.Destroy();
        }

        protected override ProjectileController CreateProjectile()
        {

            NetworkProjectileView view = m_Pool.GetProjectile();
            ProjectileController pc = new ProjectileController(chargedFiringData, view);
            view.SetController(pc);
            return pc;
        }

        protected override void InvokeShootAudioEvent()
        {
            EventBus<NetworkAudioEventData>.Invoke(new NetworkAudioEventData
            {
                OwnerClientOnly = false,
                FollowNetworkObject = false,
                AudioTagNetworkGuid = chargedFiringData.Tag.Guid.ToNetworkGuid(),
                Position = actor.FirePoint.position
            });
        }
    }
}
