using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace BTG.Tank.UltimateAction
{
    public class AirStrike : IUltimateAction
    {
        private AirStrikeDataSO m_AirStrikeData;

        private AirStrikeView m_View;

        private float m_TimeElapsed;

        private CancellationTokenSource m_CancellationTokenSource;

        public AirStrike(AirStrikeDataSO airStrikeData)
        {
            m_AirStrikeData = airStrikeData;
            m_CancellationTokenSource = new CancellationTokenSource();
        }

        public float Duration => m_AirStrikeData.Duration;

        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: AirStrike executed");
            SpawnVFX(controller.Transform);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            _ = DestroyAfterDuration(m_CancellationTokenSource.Token);
        }

        public void OnDestroy()
        {
            m_CancellationTokenSource.Cancel();
        }

        private void SpawnVFX(Transform parent)
        {
            m_View = Object.Instantiate(m_AirStrikeData.AirStrikeViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }

        private async Task DestroyAfterDuration(CancellationToken cancellationToken)
        {
            m_TimeElapsed = 0;

            try
            {
                while (m_TimeElapsed < m_AirStrikeData.Duration)
                {
                    await Task.Delay(1000);
                    m_TimeElapsed += 1;
                }

                m_View.StopParticleSystem();
                m_View.StopAudio();
                Object.Destroy(m_View.gameObject);
                m_View = null;
            }
            catch (TaskCanceledException)
            {
                // Do nothing
            }
        }
    }
}
