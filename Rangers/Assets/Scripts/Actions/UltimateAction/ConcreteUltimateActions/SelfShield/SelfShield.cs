using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    public class SelfShield : BaseUltimateAction
    {
        public override event System.Action OnFullyCharged;

        protected SelfShieldDataSO selfShieldData => ultimateActionData as SelfShieldDataSO;

        protected SelfShieldView m_View;

        public SelfShield(SelfShieldDataSO selfShieldData)
        {
            ultimateActionData = selfShieldData;
        }

        public override void Disable()
        {
            if (m_View != null)
            {
                Object.Destroy(m_View.gameObject);
            }

            base.Disable();
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
            {
                return false;
            }

            ChangeState(State.Executing);
            InitVisual();
            RestartAfterDuration(selfShieldData.Duration);

            return true;
        }

        public override void Destroy()
        {
            OnFullyCharged = null;
            DeInitVisual();
            base.Destroy();
        }

        protected override void Restart()
        {
            DeInitVisual();
            RaiseUltimateActionExecutedEvent();
            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
        }

        private void InitVisual()
        {
            m_View = Object.Instantiate(selfShieldData.SelfShieldViewPrefab, Actor.Transform);
            m_View.SetOwner(Actor.Transform, Actor.IsPlayer);

            InvokeEffectEvent();
        }

        private void DeInitVisual()
        {
            if (m_View == null)
            {
                return;
            }

            Object.Destroy(m_View.gameObject);
        }

        private void InvokeEffectEvent()
        {
            if (Actor.IsNetworkPlayer)
            {
                EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData
                {
                    FollowNetworkObject = true,
                    FollowNetowrkObjectId = Actor.NetworkObjectId,
                    TagNetworkGuid = selfShieldData.Tag.Guid.ToNetworkGuid(),
                    Duration = selfShieldData.Duration
                });
            }
            else
            {
                EventBus<EffectEventData>.Invoke(new EffectEventData
                {
                    FollowTarget = Actor.Transform,
                    Tag = selfShieldData.Tag,
                    Duration = selfShieldData.Duration
                });
            }
        }
    }
}
