using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public class NetworkAutoTarget : AutoTarget
    {
        public NetworkAutoTarget(AutoTargetDataSO autoTargetData) : base(autoTargetData)
        {
        }

        /// <summary>
        /// In case of Multiplayer - Server side explosion creation event invoked by the projectile on collision.
        /// </summary>
        public override void CreateExplosion(Vector3 position)
        {
            EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData
            {
                OwnerClientOnly = false,
                FollowNetworkObject = false,
                EffectTagNetworkGuid = autoTargetData.ExplosionTag.Guid.ToNetworkGuid(),
                EffectPosition = position,
            });
        }

        protected override void SpawnConfigureLaunchProjectile(Transform targetTransform)
        {
            AutoTargetView projectile = Object.Instantiate(autoTargetData.AutoTargetViewPrefab, Actor.FirePoint.position, Actor.FirePoint.rotation);
            
            GameObject go = projectile.gameObject.CreateNetworkObject();
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

            projectile.Configure(this, targetTransform, autoTargetData.ProjectileSpeed, Actor.Transform);
            projectile.GetComponent<NetworkObject>().Spawn(true);
            projectile.Launch();
        }
    }
}
