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

        protected override ProjectileController CreateProjectile()
        {
            NetworkProjectileView view = m_Pool.GetProjectile();
            ProjectileController pc = new ProjectileController(chargedFiringData, view);
            view.SetController(pc);
            view.SetActorOwnerClientId(actor.OwnerClientId);
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
