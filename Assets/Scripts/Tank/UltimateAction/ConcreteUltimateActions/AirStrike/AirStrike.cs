using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class AirStrike : UltimateAction
    {
        private AirStrikeDataSO m_AirStrikeData => m_UltimateActionData as AirStrikeDataSO;

        private AirStrikeView m_View;

        // Create constructor
        public AirStrike(AirStrikeDataSO airStrikeData)
        {
            m_UltimateActionData = airStrikeData;
            Start();
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            if (!IsFullyCharged)
            {
                return false;
            }

            SpawnView(controller.Transform);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            _ = ResetAfterDuration(m_CancellationTokenSource.Token);

            RaiseCameraShakeEvent(m_AirStrikeData.Duration);

            return true;
        }

        protected override void Reset()
        {
            m_View.StopParticleSystem();
            m_View.StopAudio();
            Object.Destroy(m_View.gameObject);
            m_View = null;

            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        private void SpawnView(Transform parent)
        {
            m_View = Object.Instantiate(m_AirStrikeData.AirStrikeViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }
    }
}
