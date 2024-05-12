using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    public class AutoTarget : UltimateAction
    {
        public override event System.Action OnFullyCharged;

        private AutoTargetDataSO m_AutoTargetData => m_UltimateActionData as AutoTargetDataSO;

        public AutoTarget(IUltimateActor controller, AutoTargetDataSO autoTargetData)
        {
            Actor = controller;
            m_UltimateActionData = autoTargetData;
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
                return false;

            if (!ScanForNearbyColliders(out Collider[] results))
                return false;

            FilterDamageables(results, out List<IDamageable> damageables);
            if (damageables.Count == 0) return false;

            ChangeState(State.Executing);

            _ = FireSequenceAsync(damageables);

            return true;
        }

        public override void Destroy()
        {
            OnFullyCharged = null;
            base.Destroy();
        }

        protected override void Restart()
        {
            RaiseUltimateActionExecutedEvent();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        public void OnHitDamageable(IDamageable damageable) => damageable.TakeDamage(m_AutoTargetData.Damage);

        public void CreateExplosion(Vector3 position) => m_AutoTargetData.ExplosionFactory.CreateExplosion(position);

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
        }

        private bool ScanForNearbyColliders(out Collider[] results)
        {
            results = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(
                (Actor.Transform.position + Actor.Transform.forward * m_AutoTargetData.CenterOffset),
                m_AutoTargetData.ImpactRadius,
                results,
                Actor.OppositionLayerMask,
                QueryTriggerInteraction.Ignore);

            if (count <= 0)
                return false;

            return true;
        }

        private void FilterDamageables(in Collider[] results, out List<IDamageable> damageables)
        {
            damageables = new List<IDamageable>();

            for (int i = 0, count = results.Length; i < count; i++)
            {
                if (results[i] == null)
                    continue;

                if (results[i].TryGetComponent(out IDamageable damageable))
                {
                    if (damageable == Actor.Damageable)
                    {
                        continue;
                    }
                    damageables.Add(damageable);
                }
            }
        }

        private async Task FireSequenceAsync(List<IDamageable> damageables)
        {
            try
            {
                foreach (IDamageable damageable in damageables)
                {
                    SpawnConfigureLaunchProjectile(damageable.Transform);

                    // Actor.ShakePlayerCamera(1f, 1f);
                    if (Actor.IsPlayer)
                        EventBus<CameraShakeEvent>.Invoke(new CameraShakeEvent { ShakeAmount = 1f, ShakeDuration = 1f });
                    // Do audio and visual effects here
                    // Do camera shake here
                    await Task.Delay((1 / m_AutoTargetData.FireRate) * 1000, m_CTS.Token);
                }

                // after all tanks are targeted, reset the ultimate
                Restart();
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
        }

        private void SpawnConfigureLaunchProjectile(in Transform targetTransform)
        {
            AutoTargetView projectile = Object.Instantiate(m_AutoTargetData.AutoTargetViewPrefab, Actor.FirePoint.position, Actor.FirePoint.rotation);
            projectile.Configure(this, targetTransform, m_AutoTargetData.ProjectileSpeed);
            projectile.Launch();
        }
    }
}
