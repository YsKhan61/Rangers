using System.Threading.Tasks;
using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public class AirStrike : IUltimateAction
    {
        private AirStrikeDataSO m_AirStrikeData;

        private AirStrikeView m_View;

        private float m_TimeElapsed;

        public AirStrike(AirStrikeDataSO airStrikeData)
        {
            m_AirStrikeData = airStrikeData;
        }

        public void Execute(TankUltimateController controller)
        {
            Debug.Log("Ultimate: AirStrike executed");
            SpawnVFX(controller.Transform);
            DestroyAfterDuration();
        }

        private void SpawnVFX(Transform parent)
        {
            m_View = Object.Instantiate(m_AirStrikeData.AirStrikeViewPrefab, parent);
            m_View.transform.localPosition = Vector3.zero;
            m_View.transform.localRotation = Quaternion.identity;
        }

        public async void DestroyAfterDuration()
        {
            m_TimeElapsed = 0;

            while (m_TimeElapsed < m_AirStrikeData.Duration)
            {
                await Task.Delay(1000);
                m_TimeElapsed += 1;
            }

            Object.Destroy(m_View.gameObject);
            m_View = null;
        }
    }
}
