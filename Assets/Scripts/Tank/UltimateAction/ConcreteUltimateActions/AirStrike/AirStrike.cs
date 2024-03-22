using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace BTG.Tank.UltimateAction
{
    public class AirStrike : UltimateAction
    {
        private AirStrikeDataSO m_AirStrikeData => m_UltimateActionData as AirStrikeDataSO;

        private AirStrikeView m_View;

        private float m_TimeElapsedSinceExecution;

        private CancellationTokenSource m_CancellationTokenSource;

        // Create constructor
        public AirStrike(AirStrikeDataSO airStrikeData)
        {
            m_UltimateActionData = airStrikeData;
            m_CancellationTokenSource = new CancellationTokenSource();

            Charge(-FULL_CHARGE);

            RaiseUltimateActionAssignedEvent();
        }

        public override bool TryExecute(TankUltimateController controller)
        {
            if (!IsFullyCharged)
            {
                return false;
            }

            SpawnVFX(controller.Transform);
            m_View.PlayParticleSystem();
            m_View.PlayAudio();
            _ = DestroyAfterDuration(m_CancellationTokenSource.Token);

            RaiseUltimateExecutedEvent(m_AirStrikeData.Duration);

            Charge(-FULL_CHARGE);

            return true;
        }

        public override void OnDestroy()
        {
            m_CancellationTokenSource.Cancel();
            base.OnDestroy();
        }

        private void SpawnVFX(Transform parent)
        {
            m_View = Object.Instantiate(m_AirStrikeData.AirStrikeViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }

        private async Task DestroyAfterDuration(CancellationToken cancellationToken)
        {
            m_TimeElapsedSinceExecution = 0;

            try
            {
                while (m_TimeElapsedSinceExecution < m_AirStrikeData.Duration)
                {
                    await Task.Delay(1000, cancellationToken);
                    m_TimeElapsedSinceExecution += 1;
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
