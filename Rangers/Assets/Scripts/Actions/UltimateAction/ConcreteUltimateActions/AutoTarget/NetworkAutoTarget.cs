using BTG.Utilities;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BTG.Actions.UltimateAction
{
    public class NetworkAutoTarget : AutoTarget
    {
        public NetworkAutoTarget(AutoTargetDataSO autoTargetData) : base(autoTargetData)
        {
        }

        public override void NonServerExecute()
        {
            if (!ScanForNearbyColliders(out Collider[] results))
                return;

            FilterDamageables(results, out List<IDamageableView> damageables);
            if (damageables.Count == 0) return;

            _ = FireProjectileInSequenceAsync(damageables);
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
