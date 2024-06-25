using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using Unity.Netcode;
using UnityEngine;

namespace BTG.Actions.UltimateAction
{
    public class NetworkSelfShield : SelfShield
    {
        public NetworkSelfShield(SelfShieldDataSO selfShieldData) : base(selfShieldData)
        {
        }

        protected override void InitVisual()
        {
            m_View = Object.Instantiate(selfShieldData.NetworkSelfShieldViewPrefab, Actor.Transform);
            m_View.SetOwner(Actor.Transform, Actor.IsPlayer);
            m_View.GetComponent<NetworkObject>().Spawn(true);

            EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData
            {
                FollowNetworkObject = true,
                FollowNetowrkObjectId = Actor.NetworkObjectId,
                EffectTagNetworkGuid = selfShieldData.Tag.Guid.ToNetworkGuid(),
                Duration = selfShieldData.Duration
            });

            // Invoke effect sound
        }

        protected override void DeInitVisual()
        {
            m_View.GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
