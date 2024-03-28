using BTG.Utilities;
using System.Collections.Generic;
using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;


namespace BTG.Tank.UltimateAction
{
    public class AirStrike : UltimateAction, ICameraShakeUltimateAction
    {
        public event System.Action<float> OnExecuteCameraShake;

        private AirStrikeDataSO m_AirStrikeData => m_UltimateActionData as AirStrikeDataSO;

        private AirStrikeView m_View;

        private Collider[] m_OverlappingColliders = new Collider[10];
        private readonly List<IDamageable> m_Damageables = new();

        // Create constructor
        public AirStrike(TankUltimateController controller, AirStrikeDataSO airStrikeData)
        {
            m_UltimateController = controller;
            m_UltimateActionData = airStrikeData;
            Start();
        }

        public void FixedUpdate()
        {
            UpdateExecution();
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
            {
                return false;
            }

            ChangeState(State.Executing);

            SpawnView(m_UltimateController.TankTransform);
            m_View.SetController(this);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            _ = ResetAfterDuration(m_AirStrikeData.Duration, m_CancellationTokenSource.Token);

            OnExecuteCameraShake?.Invoke(m_AirStrikeData.Duration);

            return true;
        }

        protected override void Reset()
        {
            m_View.StopParticleSystem();
            m_View.StopAudio();
            Object.Destroy(m_View.gameObject);
            m_View = null;

            RaiseUltimateActionExecutedEvent();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);               // Reset charge
            AutoCharge();
        }

        public override void OnDestroy()
        {
            OnExecuteCameraShake = null;
            base.OnDestroy();
        }

        private void SpawnView(Transform parent)
        {
            m_View = Object.Instantiate(m_AirStrikeData.AirStrikeViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }


        private void UpdateExecution()
        {
            if (CurrentState != State.Executing)
            {
                return;
            }

            if (!CheckNearbyDamageables())
            {
                return;
            }

            FetchDamageables();
            DamageDamageables();
        }

        private bool CheckNearbyDamageables()
        {
            int count = Physics.OverlapSphereNonAlloc(
                m_UltimateController.TankTransform.position,
                m_AirStrikeData.ImpactRadius,
                m_OverlappingColliders,
                m_AirStrikeData.LayerMask,
                QueryTriggerInteraction.Ignore);

            return count > 0;
        }

        private void FetchDamageables()
        {
            m_Damageables.Clear();
            for (int i = 0, count = m_OverlappingColliders.Length; i < count; i++)
            {
                if (m_OverlappingColliders[i] == null)
                {
                    continue;
                }

                if (m_OverlappingColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    m_Damageables.Add(damageable);
                }
            }
        }

        private void DamageDamageables()
        {
            if (m_Damageables.Count == 0)
            {
                return;
            }

            foreach (IDamageable damageable in m_Damageables)
            {
                if (damageable == m_UltimateController.Damageable)
                {
                    continue;
                }

                damageable.TakeDamage(m_AirStrikeData.Damage);
            }
        }
    }
}
