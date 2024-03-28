using BTG.Utilities;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;

namespace BTG.Tank.UltimateAction
{
    public class AutoTarget : UltimateAction, ICameraShakeUltimateAction
    {
        public event System.Action<float> OnExecuteCameraShake;

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

            if (!TryGetNearbyTanks(out TankView[] tanks))
                return false;

            ChangeState(State.Executing);

            _ = FireSequenceAsync(tanks);

            return true;
        }

        public override void OnDestroy()
        {
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

        private bool TryGetNearbyTanks(out TankView[] views)
        {
            views = null;

            Collider[] results = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(
                (m_UltimateController.TankTransform.position + m_UltimateController.TankTransform.forward * m_AutoTargetData.CenterOffset),
                m_AutoTargetData.ImpactRadius,
                results,
                m_AutoTargetData.TargetLayer,
                QueryTriggerInteraction.Ignore);

            if (count < 0)
                return false;

            FilterTankViews(count, results, out views);
            if (views.Length <= 0)
                return false;

            return true;
        }

        private void FilterTankViews(in int count, in Collider[] results, out TankView[] views)
        {
            TankView[] temp = new TankView[count];

            for (int i = 0; i < count; i++)
            {
                if (results[i].TryGetComponent(out TankView tankView))
                {
                    if (tankView.Controller == m_UltimateController.TankController)
                    {
                        continue;
                    }
                    temp[i] = tankView;
                }
            }

            views = temp.Where(item => item != null).ToArray();
        }

        private async Task FireSequenceAsync(TankView[] tanks)
        {
            try
            {
                foreach (TankView tank in tanks)
                {
                    SpawnAutoTargetProjectile(out AutoTargetView projectile);
                    projectile.Configure(this, tank.transform, m_AutoTargetData.ProjectileSpeed);
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
