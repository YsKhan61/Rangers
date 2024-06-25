using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    public class Invisibility : BaseUltimateAction
    {
        public override event System.Action OnFullyCharged;

        private InvisibilityDataSO m_InvisibilityData => ultimateActionData as InvisibilityDataSO;

        public Invisibility(InvisibilityDataSO invisibilityData)
        {
            ultimateActionData = invisibilityData;
        }

        public override void Disable()
        {
            base.Disable();
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
            {
                return false;
            }

            ChangeState(State.Executing);

            ReadyToHideVisual();

            RestartAfterDuration(m_InvisibilityData.Duration);

            return true;
        }

        public override void Destroy()
        {
            OnFullyCharged = null;
            base.Destroy();
        }

        protected override void Restart()
        {
            InvokeEffectEvent(m_InvisibilityData.VisibleEffectTag);

            _ = HelperMethods.InvokeAfterAsync((int)m_InvisibilityData.VisibleDelay, () =>
            {
                ReadyToShowVisual();
                RaiseUltimateActionExecutedEvent();
                ChangeState(State.Charging);
                Charge(-FULL_CHARGE);
                AutoCharge();
            }, 
            cts.Token);
        }

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
        }

        private void ReadyToHideVisual()
        {
            InvokeEffectEvent(m_InvisibilityData.InvisibleEffectTag);
            Actor.ToggleActorVisibility(false);
        }

        private void ReadyToShowVisual()
        {
            Actor.ToggleActorVisibility(true);
            InvokeCameraShakeEvent();
        }

        private void InvokeEffectEvent(TagSO effectTag)
        {
            if (Actor.IsNetworkPlayer)
            {
                EventBus<NetworkEffectEventData>.Invoke(new NetworkEffectEventData
                {
                    OwnerClientOnly = false,
                    FollowNetworkObject = true,
                    FollowNetowrkObjectId = Actor.NetworkObjectId,
                    EffectTagNetworkGuid = effectTag.Guid.ToNetworkGuid(),
                });
            }
            else
            {
                EventBus<EffectEventData>.Invoke(new EffectEventData
                {
                    EffectTag = effectTag,
                    FollowTarget = Actor.Transform,
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
                    ShakeDuration = 1f
                });
            }
        }
    }
}
