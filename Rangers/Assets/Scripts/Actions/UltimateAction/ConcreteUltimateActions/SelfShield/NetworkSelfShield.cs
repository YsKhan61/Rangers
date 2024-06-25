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
            m_View = Object.Instantiate(selfShieldData.SelfShieldViewPrefab, Actor.Transform);
            m_View.SetOwner(Actor.Transform, Actor.IsPlayer);

            GameObject go = m_View.gameObject.CreateNetworkObject();
            go.CreateNetworkTransform(new NetworkTransformSettings 
            { 
                SyncPositionX = true,
                SyncPositionY = true,
                SyncPositionZ = true,

                SyncRotAngleX = false,
                SyncRotAngleY = false,
                SyncRotAngleZ = false,

                SyncScaleX = false, 
                SyncScaleY = false, 
                SyncScaleZ = false 
            });
            
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
