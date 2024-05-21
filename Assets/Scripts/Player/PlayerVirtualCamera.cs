using BTG.Events;
using BTG.Utilities.EventBus;
using Cinemachine;
using System.Collections;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerVirtualCamera : MonoBehaviour
    {
        EventBinding<CameraShakeEvent> m_CameraShakeEventBinding;

        [SerializeField] CinemachineVirtualCamera m_PVC1;

        [SerializeField] float m_MinShakeIntensity;
        [SerializeField] float m_MaxShakeIntensity;

        private CinemachineBasicMultiChannelPerlin m_Perlin;
        private WaitForSeconds m_WaitForSeconds;
        private Coroutine m_StopShakeCoroutine;

        private void Awake()
        {
            m_Perlin = m_PVC1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            ResetProperties();
        }

        private void OnEnable()
        {
            m_CameraShakeEventBinding = new EventBinding<CameraShakeEvent>(OnPlayerCamShake);
            EventBus<CameraShakeEvent>.Register(m_CameraShakeEventBinding);
        }

        private void OnDisable()
        {
            EventBus<CameraShakeEvent>.Unregister(m_CameraShakeEventBinding);
        }
        

        public void SetTarget(Transform target) => m_PVC1.Follow = target;

        private void OnPlayerCamShake(CameraShakeEvent data)
            => StartShake(Mathf.Max(m_MinShakeIntensity, m_MaxShakeIntensity * data.ShakeAmount), data.ShakeDuration);

        void StartShake(float intensity, float duration)
        {
            if (m_StopShakeCoroutine != null)
            {
                StopCoroutine(m_StopShakeCoroutine);
                ResetProperties();
            }

            m_Perlin.m_AmplitudeGain = intensity;
            m_WaitForSeconds = new WaitForSeconds(duration);
            m_StopShakeCoroutine = StartCoroutine(StopShake());
        }

        IEnumerator StopShake()
        {
            yield return m_WaitForSeconds;
            ResetProperties();
        }

        private void ResetProperties()
        {
            m_Perlin.m_AmplitudeGain = 0;
            m_WaitForSeconds = null;
            m_StopShakeCoroutine = null;
        }
    }
}

