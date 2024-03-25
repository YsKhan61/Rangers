using System.Threading.Tasks;
using UnityEngine;
using State = BTG.Tank.UltimateAction.IUltimateAction.State;

namespace BTG.Tank.UltimateAction
{
    public class Invisibility : UltimateAction
    {
        public event System.Action<float> OnExecuteCameraShake;

        private InvisibilityDataSO m_InvisibilityData => m_UltimateActionData as InvisibilityDataSO;

        private InvisibilityView m_View;

        public Invisibility(TankUltimateController controller, InvisibilityDataSO invisibilityData)
        {
            m_UltimateController = controller;
            m_UltimateActionData = invisibilityData;
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
            m_View.PlayDisappearPS();
            m_View.PlayDisappearAudio();
            m_UltimateController.TankController.HideGraphics();
            _ = ResetAfterDuration(m_InvisibilityData.Duration, m_CancellationTokenSource.Token);

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
                m_UltimateController.TankController.ShowGraphics();
                OnExecuteCameraShake?.Invoke(1f);

                RaiseUltimateActionExecutedEvent();

                ChangeState(State.Charging);
                Charge(-FULL_CHARGE);
                AutoCharge();
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
            
        }
    }
}
