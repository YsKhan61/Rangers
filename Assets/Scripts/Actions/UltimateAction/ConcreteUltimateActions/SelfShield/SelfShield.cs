using UnityEngine;
using State = BTG.Actions.UltimateAction.IUltimateAction.State;


namespace BTG.Actions.UltimateAction
{
    public class SelfShield : BaseUltimateAction
    {
        public override event System.Action OnFullyCharged;

        private SelfShieldDataSO m_SelfShieldData => m_UltimateActionData as SelfShieldDataSO;

        private SelfShieldView m_View;

        public SelfShield(SelfShieldDataSO selfShieldData)
        {
            m_UltimateActionData = selfShieldData;
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
            {
                return false;
            }

            SpawnView(Actor.Transform);
            m_View.SetOwner(Actor.Transform, Actor.IsPlayer);
            m_View.SetParticleSystem(m_SelfShieldData.Duration);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            RestartAfterDuration(m_SelfShieldData.Duration);

            return true;
        }

        public override void Destroy()
        {
            OnFullyCharged = null;
            base.Destroy();
        }

        protected override void Restart()
        {
            m_View.StopParticleSystem();
            Object.Destroy(m_View.gameObject);
            m_View = null;

            RaiseUltimateActionExecutedEvent();

            ChangeState(State.Charging);
            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        protected override void RaiseFullyChargedEvent()
        {
            OnFullyCharged?.Invoke();
        }

        private void SpawnView(Transform parent)
        {
            m_View = Object.Instantiate(m_SelfShieldData.SelfShieldViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }
    }
}
