using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using Cinemachine;
using System.Collections;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerVirtualCamera : MonoBehaviour
    {
        EventBinding<CameraShakeEvent> m_CameraShakeEventBinding;

        [SerializeField]
        FloatFloatEventChannelSO m_OnPlayerCamShake;

        [SerializeField] CinemachineVirtualCamera m_PVC1;

        [SerializeField] float m_MinShakeIntensity;
        [SerializeField] float m_MaxShakeIntensity;

        private CinemachineBasicMultiChannelPerlin m_Perlin;
        private WaitForSeconds m_WaitForSeconds;
        private Coroutine m_StopShakeCoroutine;

        private void OnEnable()
        {
            m_CameraShakeEventBinding = new EventBinding<CameraShakeEvent>(OnPlayerCamShake);
            EventBus<CameraShakeEvent>.Register(m_CameraShakeEventBinding);
            // m_OnPlayerCamShake.OnEventRaised += OnPlayerCamShake;
        }

        private void OnDisable()
        {
            EventBus<CameraShakeEvent>.Unregister(m_CameraShakeEventBinding);
            // m_OnPlayerCamShake.OnEventRaised -= OnPlayerCamShake;
        }
        

        public void Initialize(Transform target)
        {
            m_PVC1.Follow = target;
            m_Perlin = m_PVC1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            Reset();
        }

        private void OnPlayerCamShake(float amount, float duration)
        {
            StartShake(Mathf.Max(m_MinShakeIntensity, m_MaxShakeIntensity * amount),
                duration);
        }

        private void OnPlayerCamShake(CameraShakeEvent data)
        {
            StartShake(Mathf.Max(m_MinShakeIntensity, m_MaxShakeIntensity * data.ShakeAmount),
                               data.ShakeDuration);
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

