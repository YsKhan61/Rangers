using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class SelfShield : UltimateAction
    {
        private SelfShieldDataSO m_SelfShieldData => m_UltimateActionData as SelfShieldDataSO;

        private SelfShieldView m_View;

        public SelfShield(SelfShieldDataSO selfShieldData)
        {
            m_UltimateActionData = selfShieldData;
            Start();
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            if (!IsFullyCharged)
            {
                return false;
            }

            SpawnView(controller.Transform);
            m_View.SetParticleSystem(m_SelfShieldData.Duration);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            _ = ResetAfterDuration(m_CancellationTokenSource.Token);

            return true;
        }

        protected override void Reset()
        {
            m_View.StopParticleSystem();
            Object.Destroy(m_View.gameObject);
            m_View = null;

            Charge(-FULL_CHARGE);
            AutoCharge();
        }

        private void SpawnView(Transform parent)
        {
            m_View = Object.Instantiate(m_SelfShieldData.SelfShieldViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }
    }
}
