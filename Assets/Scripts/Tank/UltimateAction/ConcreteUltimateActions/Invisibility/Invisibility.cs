using System.Threading.Tasks;
using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class Invisibility : UltimateAction
    {
        private InvisibilityDataSO m_InvisibilityData => m_UltimateActionData as InvisibilityDataSO;

        private InvisibilityView m_View;

        private TankUltimateController m_Controller;

        public Invisibility(InvisibilityDataSO invisibilityData)
        {
            m_UltimateActionData = invisibilityData;
            Start();
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            if (!IsFullyCharged)
            {
                return false;
            }

            if (m_Controller == null || m_Controller != controller)
            {
                m_Controller = controller;
            }

            SpawnView(controller.Transform);
            m_View.PlayDisappearPS();
            m_View.PlayDisappearAudio();
            controller.HideTankView();
            _ = ResetAfterDuration(m_CancellationTokenSource.Token);

            return true;
        }

        protected override void Reset()
        {
            m_View.PlayAppearPS();
            m_View.PlayAppearAudio();

            _ = ResetAfterDelay(m_CancellationTokenSource.Token);
        }

        private void SpawnView(Transform parent)
        {
            m_View = Object.Instantiate(m_InvisibilityData.InvisibilityViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }

        private async Task ResetAfterDelay(System.Threading.CancellationToken token)
        {
            try
            {
                await Task.Delay((int)(m_View.AppearPSDuration * 1000), token);
                Object.Destroy(m_View.gameObject);
                m_View = null;
                m_Controller.ShowTankView();

                Charge(-FULL_CHARGE);
                AutoCharge();
                RaiseCameraShakeEvent(1f);
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
            
        }
    }
}
