using BTG.Utilities;
using UnityEngine;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    public class SelfShield : BaseUltimateAction
    {
        public override event System.Action OnFullyCharged;

        private SelfShieldDataSO m_SelfShieldData => ultimateActionData as SelfShieldDataSO;

        private SelfShieldView m_View;

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
            RestartAfterDuration(m_SelfShieldData.Duration);

            return true;
        }

        public override void NonServerExecute()
        {
            cts = new();
            InitVisual();
            _ = HelperMethods.InvokeAfterAsync(m_SelfShieldData.Duration,
                () => 
                {
                    DeInitVisual();
                }, 
                cts.Token);
        }

        public override void Destroy()
        {
            OnFullyCharged = null;
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
            m_View = Object.Instantiate(m_SelfShieldData.SelfShieldViewPrefab, Actor.Transform);
            m_View.SetOwner(Actor.Transform, Actor.IsPlayer);
            m_View.SetParticleSystem(m_SelfShieldData.Duration);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
        }

        private void DeInitVisual()
        {
            m_View.StopParticleSystem();
            Object.Destroy(m_View.gameObject);
            m_View = null;
        }
    }
}
