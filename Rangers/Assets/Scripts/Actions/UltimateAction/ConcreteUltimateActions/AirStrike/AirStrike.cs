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

        private AirStrikeDataSO m_AirStrikeData => ultimateActionData as AirStrikeDataSO;

        private Collider[] m_OverlappingColliders = new Collider[10];
        private readonly List<IDamageableView> m_Damageables = new();

        public AirStrike(AirStrikeDataSO airStrikeData)
        {
            ultimateActionData = airStrikeData;
        }

        public override void Enable()
        {
            base.Enable();
            UnityMonoBehaviourCallbacks.Instance.RegisterToFixedUpdate(this);
        }

        public override void Disable()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this);

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
            InvokeEffectEvent();
            RestartAfterDuration(m_AirStrikeData.Duration);
            InvokeCameraShakeEvent();
            
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
            Charge(-FULL_CHARGE);               // Reset charge
            AutoCharge();
        }

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
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

                if (m_OverlappingColliders[i].TryGetComponent(out IDamageableView damageable))
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

            foreach (IDamageableView damageable in m_Damageables)
            {
                if (damageable == Actor.Damageable)
                {
                    continue;
                }

                damageable.Damage(m_AirStrikeData.Damage);
            }
        }

        private void InvokeEffectEvent()
        {
            if (Actor.IsNetworkPlayer)
            {
                EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData 
                { 
                    OwnerClientOnly = false,
                    FollowNetworkObject = true,
                    FollowNetowrkObjectId = Actor.NetworkObjectId,
                    TagNetworkGuid = m_AirStrikeData.Tag.Guid.ToNetworkGuid(),
                    Duration = m_AirStrikeData.Duration
                });
            }
            else
            {
                EventBus<EffectEventData>.Invoke(new EffectEventData 
                { 
                    Tag = m_AirStrikeData.Tag, 
                    Position = Actor.Transform.position, 
                    Duration = m_AirStrikeData.Duration 
                });
            }
        }

        private void InvokeCameraShakeEvent()
        {
            if (Actor.IsPlayer)
            {
                Actor.RaisePlayerCamShakeEvent(new CameraShakeEventData
                {
                    ShakeAmount = 1f,
                    ShakeDuration = m_AirStrikeData.Duration
                });
            }
        }
    }
}
