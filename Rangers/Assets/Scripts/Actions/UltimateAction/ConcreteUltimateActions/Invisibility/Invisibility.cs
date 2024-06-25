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

        // private InvisibilityView m_View;

        public Invisibility(InvisibilityDataSO invisibilityData)
        {
            ultimateActionData = invisibilityData;
        }

        public override void Disable()
        {
            /*if (m_View != null)
            {
                Object.Destroy(m_View.gameObject);
            }*/

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
            /*m_View.PlayAppearPS();
            m_View.PlayAppearAudio();*/

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

        /*private void SpawnView(Transform parent)
        {
            m_View = Object.Instantiate(m_InvisibilityData.InvisibilityViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }

        private void DoAfterDelay()
        {
            ReadyToShowVisual();
            RaiseUltimateActionExecutedEvent();
            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        private void DeInitVisual1()
        {
            m_View.PlayAppearPS();
            m_View.PlayAppearAudio();

            _ = HelperMethods.InvokeAfterAsync((int)m_View.AppearPSDuration, () => DeInitVisual2(), cts.Token);
        }*/

        private void ReadyToHideVisual()
        {
            /*SpawnView(Actor.Transform);
            m_View.PlayDisappearPS();
            m_View.PlayDisappearAudio();*/
            InvokeEffectEvent(m_InvisibilityData.InvisibleEffectTag);
            Actor.ToggleActorVisibility(false);
        }

        private void ReadyToShowVisual()
        {
            /*Object.Destroy(m_View.gameObject);
            m_View = null;*/
            Actor.ToggleActorVisibility(true);

            /*if (Actor.IsPlayer)
                Actor.RaisePlayerCamShakeEvent(new CameraShakeEventData { ShakeAmount = 1f, ShakeDuration = 1f });*/
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
