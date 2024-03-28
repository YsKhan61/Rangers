using BTG.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;

namespace BTG.Tank.UltimateAction
{
    public class AutoTarget : UltimateAction, ICameraShakeUltimateAction
    {
        public event System.Action<float> OnExecuteCameraShake;
        public override event System.Action<IUltimateAction> OnFullyCharged;

        private AutoTargetDataSO m_AutoTargetData => m_UltimateActionData as AutoTargetDataSO;

        public AutoTarget(TankUltimateController controller, AutoTargetDataSO autoTargetData)
        {
            m_UltimateController = controller;
            m_UltimateActionData = autoTargetData;
            Start();
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

        public override void OnDestroy()
        {
            OnFullyCharged = null;
            OnExecuteCameraShake = null;
            base.OnDestroy();
        }

        protected override void Reset()
        {
            RaiseUltimateActionExecutedEvent();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        public void OnHitObject(Collision collision)
        {
            if (collision.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(m_AutoTargetData.Damage);
            }
        }

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke(this);
        }

        private bool ScanForNearbyColliders(out Collider[] results)
        {
            results = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(
                (m_UltimateController.TankTransform.position + m_UltimateController.TankTransform.forward * m_AutoTargetData.CenterOffset),
                m_AutoTargetData.ImpactRadius,
                results,
                m_UltimateController.LayerMask,
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
                    if (damageable == m_UltimateController.Damageable)
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
                    SpawnAutoTargetProjectile(out AutoTargetView projectile);
                    projectile.Configure(this, damageable.Transform, m_AutoTargetData.ProjectileSpeed);
                    projectile.Launch();
                    OnExecuteCameraShake?.Invoke(1f);
                    // Do audio and visual effects here
                    // Do camera shake here
                    await Task.Delay((1 / m_AutoTargetData.FireRate) * 1000, m_CancellationTokenSource.Token);
                }

                // after all tanks are targeted, reset the ultimate
                Reset();
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
        }

        private void SpawnAutoTargetProjectile(out AutoTargetView projectile)
        {
            projectile = Object.Instantiate(m_AutoTargetData.AutoTargetViewPrefab, m_UltimateController.FirePoint.position, m_UltimateController.FirePoint.rotation);
        }
    }
}
