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
                TagNetworkGuid = autoTargetData.ExplosionTag.Guid.ToNetworkGuid(),
                EffectPosition = position,
            });
        }

        protected override void SpawnConfigureLaunchProjectile(Transform targetTransform)
        {
            AutoTargetView projectile = Object.Instantiate(autoTargetData.NetworkAutoTargetView, Actor.FirePoint.position, Actor.FirePoint.rotation);
            projectile.Configure(this, targetTransform, autoTargetData.ProjectileSpeed, Actor.Transform);
            projectile.GetComponent<NetworkObject>().Spawn(true);
            projectile.Launch();
            projectile.AutoDestroy(autoTargetData.BulletDuration);
        }
    }
}
