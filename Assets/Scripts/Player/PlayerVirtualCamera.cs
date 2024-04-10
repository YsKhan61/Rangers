using BTG.Utilities;
using Cinemachine;
using System;
using System.Collections;
using UnityEngine;


namespace BTG.Player
{
    /// <summary>
    /// We need to create separate EventBus
    /// where we can subscribe to the events and call the methods of the PlayerVirualCameraController
    /// </summary>
    public class PlayerVirtualCamera : MonoBehaviour
    {
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
            m_OnPlayerCamShake.OnEventRaised += OnPlayerCamShake;
        }

        private void OnDisable()
        {
            m_OnPlayerCamShake.OnEventRaised -= OnPlayerCamShake;
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

