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
        [SerializeField] float m_ShakeTime = 0.1f;

        public void SetFollow(Transform target)
        {
            m_PVC1.Follow = target;
        }

        public void ShakeCamera(float intensityMultiplier)
        {
            m_PVC1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                Mathf.Max(m_MinShakeIntensity, m_MaxShakeIntensity * intensityMultiplier);

            StartCoroutine(StopShake());
        }

        IEnumerator StopShake()
        {
            yield return new WaitForSeconds(m_ShakeTime);
            m_PVC1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        }
    }
}

