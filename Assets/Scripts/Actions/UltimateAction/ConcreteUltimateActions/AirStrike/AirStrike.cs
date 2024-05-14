using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System.Collections.Generic;
using UnityEngine;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    public class AirStrike : BaseUltimateAction, IFixedUpdatable
    {
        public override event System.Action OnFullyCharged;

        private AirStrikeDataSO m_AirStrikeData => m_UltimateActionData as AirStrikeDataSO;

        private AirStrikeView m_View;

        private Collider[] m_OverlappingColliders = new Collider[10];
        private readonly List<IDamageable> m_Damageables = new();

        // Create constructor
        public AirStrike(IUltimateActor actor, AirStrikeDataSO airStrikeData)
        {
            Actor = actor;
            m_UltimateActionData = airStrikeData;
        }

        public override void Enable()
        {
            base.Enable();
            UnityMonoBehaviourCallbacks.Instance.RegisterToFixedUpdate(this as IFixedUpdatable);
        }

        public override void Disable()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this as IFixedUpdatable);

            if (m_View != null) 
            {
                m_View.StopParticleSystem();
                m_View.StopAudio();
                m_View = null;
            }

            base.Disable();
        }

        public void FixedUpdate()
        {
            UpdateExecution();
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
            {
                // still charging
                return false;
            }

            ChangeState(State.Executing);

            SpawnView(Actor.Transform);
            m_View.SetController(this);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            RestartAfterDuration(m_AirStrikeData.Duration);
            // Actor.ShakePlayerCamera(1f, m_AirStrikeData.Duration);
            if (Actor.IsPlayer)
                EventBus<CameraShakeEvent>.Invoke(new CameraShakeEvent { ShakeAmount = 1f, ShakeDuration = m_AirStrikeData.Duration });

            return true;
        }

        protected override void Restart()
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

        public override void Destroy()
        {
            OnFullyCharged = null;
            base.Destroy();
        }

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
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
                Actor.Transform.position,
                m_AirStrikeData.ImpactRadius,
                m_OverlappingColliders,
                Actor.OppositionLayerMask,
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
                if (damageable == Actor.Damageable)
                {
                    continue;
                }

                damageable.TakeDamage(m_AirStrikeData.Damage);
            }
        }
    }
}
