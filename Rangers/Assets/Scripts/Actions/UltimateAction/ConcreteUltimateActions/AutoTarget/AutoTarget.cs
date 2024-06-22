using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;
using Object = UnityEngine.Object;


namespace BTG.Actions.UltimateAction
{
    public class AutoTarget : BaseUltimateAction
    {
        public override event System.Action OnFullyCharged;

        private event Action<bool> OnFireSequenceComplete;

        private AutoTargetDataSO m_AutoTargetData => ultimateActionData as AutoTargetDataSO;

        public AutoTarget(AutoTargetDataSO autoTargetData)
        {
            ultimateActionData = autoTargetData;
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
                return false;

            if (!ScanForNearbyColliders(out Collider[] results))
                return false;

            FilterDamageables(results, out List<IDamageableView> damageables);
            if (damageables.Count == 0) return false;

            ChangeState(State.Executing);

            OnFireSequenceComplete += Restart;
            _ =FireSequenceAsync(damageables);

            return true;
        }

        public override void NonServerExecute()
        {
            if (!ScanForNearbyColliders(out Collider[] results))
                return;

            FilterDamageables(results, out List<IDamageableView> damageables);
            if (damageables.Count == 0) return;

            _ = FireSequenceAsync(damageables);
        }

        public override void Destroy()
        {
            OnFullyCharged = null;
            base.Destroy();
        }

        /// <summary>
        /// This restart is different from the BaseUltimateAction restart, as it contains 
        /// a parameter to determine if the action was successful or not.
        /// We are not calling RestartAfterDuration of BaseUltimateAction as
        /// we dont have any duration to wait for.
        /// </summary>
        private void Restart(bool success)
        {
            OnFireSequenceComplete -= Restart;
            
            if (success)
                RaiseUltimateActionExecutedEvent();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        public void OnHitDamageable(IDamageableView damageable) => damageable.Damage(m_AutoTargetData.Damage);

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

        private void FilterDamageables(Collider[] results, out List<IDamageableView> damageables)
        {
            damageables = new List<IDamageableView>();

            for (int i = 0, count = results.Length; i < count; i++)
            {
                if (results[i] == null)
                    continue;

                if (results[i].TryGetComponent(out IDamageableView damageable))
                {
                    if (damageable == Actor.Damageable)
                    {
                        continue;
                    }
                    damageables.Add(damageable);
                }
            }
        }

        private async Task FireSequenceAsync(List<IDamageableView> damageables)
        {
            try
            {
                foreach (IDamageableView damageable in damageables)
                {
                    SpawnConfigureLaunchProjectile(damageable.Transform);

                    if (Actor.IsPlayer)
                        EventBus<CameraShakeEvent>.Invoke(new CameraShakeEvent { ShakeAmount = 1f, ShakeDuration = 1f });
                    // Do audio and visual effects here
                    // Do camera shake here
                    await Task.Delay((1 / m_AutoTargetData.FireRate) * 1000, cts.Token);
                }

                OnFireSequenceComplete?.Invoke(true);
            }
            catch (TaskCanceledException)
            {
                OnFireSequenceComplete?.Invoke(false);
            }
        }

        private void SpawnConfigureLaunchProjectile(Transform targetTransform)
        {
            AutoTargetView projectile = Object.Instantiate(m_AutoTargetData.AutoTargetViewPrefab, Actor.FirePoint.position, Actor.FirePoint.rotation);
            projectile.Configure(this, targetTransform, m_AutoTargetData.ProjectileSpeed, Actor.Transform);
            projectile.Launch();
        }
    }
}
