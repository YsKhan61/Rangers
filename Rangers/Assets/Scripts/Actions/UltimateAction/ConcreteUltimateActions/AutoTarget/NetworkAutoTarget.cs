using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using Unity.Netcode;
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
        /// <param name="position"></param>
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
            NetworkAutoTargetView projectile = Object.Instantiate(autoTargetData.NetworkAutoTargetView, Actor.FirePoint.position, Actor.FirePoint.rotation);
            projectile.Configure(this, targetTransform, autoTargetData.ProjectileSpeed, Actor.Transform);
            
            projectile.GetComponent<NetworkObject>().Spawn(true);
            projectile.Launch();
        }
    }
}
