using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;


namespace BTG.Actions.PrimaryAction
{
    public class NetworkTeslaFiring : TeslaFiringBase
    {
        private NetworkTeslaBallPool m_Pool;

        public NetworkTeslaFiring(TeslaFiringDataSO data, NetworkTeslaBallPool pool) : base(data)
        {
            m_Pool = pool;
        }

        protected override void SpawnBall()
        {
            m_BallInCharge = m_Pool.GetTeslaBall();
            m_BallInCharge.SetTeslaFiring(this);
        }

        protected override void InvokeShootAudioEvent()
        {
            EventBus<NetworkAudioEventData>.Invoke(new NetworkAudioEventData
            {
                OwnerClientOnly = false,
                FollowNetworkObject = false,
                AudioTagNetworkGuid = Data.ShootEffectTag.Guid.ToNetworkGuid(),
                Position = actor.FirePoint.position
            });
        }
    }
}

