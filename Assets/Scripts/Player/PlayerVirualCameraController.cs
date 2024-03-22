using Cinemachine;
using System.Collections;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerVirualCameraController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera m_PVC1;

        [SerializeField] float m_MinShakeIntensity;
        [SerializeField] float m_MaxShakeIntensity;
        [SerializeField] float m_ShakeDurationOnPlayerTankShoot = 0.1f;
        [SerializeField] float m_ShakeIntensityOnUltimateExecution = 2f;

        private CinemachineBasicMultiChannelPerlin m_Perlin;
        private WaitForSeconds m_WaitForSeconds;
        private Coroutine m_StopShakeCoroutine;

        public void Initialize(Transform target)
        {
            m_PVC1.Follow = target;
            m_Perlin = m_PVC1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Reset();
        }

        public void ShakeCameraOnPlayerTankShoot(float intensityMultiplier)
        {
            StartShake(Mathf.Max(m_MinShakeIntensity, m_MaxShakeIntensity * intensityMultiplier), 
                m_ShakeDurationOnPlayerTankShoot);
        }

        public void ShakeCameraOnUltimateExecution(float duration)
        {
            StartShake(m_ShakeIntensityOnUltimateExecution, duration);
        }

        void StartShake(float intensity, float duration)
        {
            if (m_StopShakeCoroutine != null)
            {
                StopCoroutine(m_StopShakeCoroutine);
                Reset();
            }

            m_Perlin.m_AmplitudeGain = intensity;
            m_WaitForSeconds = new WaitForSeconds(duration);
            m_StopShakeCoroutine = StartCoroutine(StopShake());
        }

        IEnumerator StopShake()
        {
            yield return m_WaitForSeconds;
            Reset();
        }

        private void Reset()
        {
            m_Perlin.m_AmplitudeGain = 0;
            m_WaitForSeconds = null;
            m_StopShakeCoroutine = null;
        }
    }
}

