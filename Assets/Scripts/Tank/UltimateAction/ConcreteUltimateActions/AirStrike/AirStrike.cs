using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;


namespace BTG.Tank.UltimateAction
{
    public class AirStrike : UltimateAction, ICameraShakeUltimateAction
    {
        public event System.Action<float> OnExecuteCameraShake;

        private AirStrikeDataSO m_AirStrikeData => m_UltimateActionData as AirStrikeDataSO;

        private AirStrikeView m_View;

        // Create constructor
        public AirStrike(TankUltimateController controller, AirStrikeDataSO airStrikeData)
        {
            m_UltimateController = controller;
            m_UltimateActionData = airStrikeData;
            Start();
        }

        public override bool TryExecute()
        {
            if (CurrentState != State.FullyCharged)
            {
                return false;
            }

            ChangeState(State.Executing);

            SpawnView(m_UltimateController.TankTransform);
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

            ChangeState(State.Executing);
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
    }
}
