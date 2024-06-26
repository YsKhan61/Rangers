using BTG.Events;
using BTG.Utilities;
using System.Threading;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    public class NetworkTeslaFiring : IPrimaryAction, IUpdatable
    {
        private const string FIRING_AUDIO_SOURCE_NAME = "FiringAudioSource";

        public event System.Action OnPrimaryActionExecuted;

        private TeslaFiringDataSO m_Data;
        public TeslaFiringDataSO Data => m_Data;

        private IPrimaryActor m_Actor;
        private NetworkTeslaBallPool m_TeslaBallPool;
        // private AudioSource m_FiringAudioSource;
        private NetworkTeslaBallView m_BallInCharge;
        private CancellationTokenSource m_Cts;

        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;



        public NetworkTeslaFiring(TeslaFiringDataSO data, NetworkTeslaBallPool pool)
        {
            m_Data = data;
            m_TeslaBallPool = pool;
        }

        public void Enable()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            // InitializeFiringAudio();

            m_IsEnabled = true;
        }

        public void Update()
        {
            if (!m_IsEnabled)
            {
                return;
            }

            UpdateChargeAmount();
            UpdateBallSize();
            ShootOnFullyCharged();
        }

        public void Disable()
        {
            // DeInitializeFiringAudio();
            m_IsEnabled = false;
            m_Cts?.Cancel();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
        }

        public void SetActor(IPrimaryActor actor) => m_Actor = actor;

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;
            PlayChargingClip();

            SpawnBall();
        }

        public void StopAction()
        {
            if (!m_IsEnabled || !m_IsCharging || m_BallInCharge == null)
                return;

            m_IsCharging = false;

            SetDamageToBallAndShoot();

            OnPrimaryActionExecuted?.Invoke();

            if (m_Actor.IsPlayer)
                m_Actor.RaisePlayerCamShakeEvent(new CameraShakeEventData { ShakeAmount = m_ChargeAmount, ShakeDuration = 0.5f });
            // EventBus<CameraShakeEventData>.Invoke(new CameraShakeEventData { ShakeAmount = m_ChargeAmount, ShakeDuration = 0.5f });

            // PlayShotFiredClip();
            ResetCharging();
        }

        public void AutoStartStopAction(int stopTime)
        {
            StartAction();

            m_Cts = new CancellationTokenSource();

            _ = HelperMethods.InvokeAfterAsync(stopTime, () =>
            {
                StopAction();
            }, m_Cts.Token);
        }

        private void SetDamageToBallAndShoot()
        {
            m_BallInCharge.transform.SetParent(m_TeslaBallPool.Container);
            m_BallInCharge.Rigidbody.isKinematic = false;
            CalculateBallDamage(out int damage);
            m_BallInCharge?.SetDamage(damage);
            m_BallInCharge.AddImpulseForce(CalculateProjectileInitialSpeed());
        }

        private void ResetCharging()
        {
            m_ChargeAmount = 0;
            // StopChargingClip();
            m_BallInCharge = null;
        }

        private void UpdateChargeAmount()
        {
            if (!m_IsCharging)
                return;

            m_ChargeAmount += Time.deltaTime / m_Data.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);
            // UpdateChargingClipPitch(m_ChargeAmount);
        }

        private void UpdateBallSize()
        {
            if (m_BallInCharge == null)
                return;

            m_BallInCharge.transform.localScale = Vector3.one * Mathf.Lerp(m_Data.MinTeslaBallScale, m_Data.MaxTeslaBallScale, m_ChargeAmount);
            m_BallInCharge.Collider.radius = Mathf.Lerp(m_Data.MinTeslaBallScale, m_Data.MaxTeslaBallScale, m_ChargeAmount);
        }

        private void ShootOnFullyCharged()
        {
            if (m_ChargeAmount >= 1)
            {
                StopAction();
            }
        }

        private void SpawnBall()
        {
            m_BallInCharge = m_TeslaBallPool.GetTeslaBall();
            m_BallInCharge.NetworkObject.Spawn(true);
            m_BallInCharge.Rigidbody.WakeUp();
            m_BallInCharge.Rigidbody.isKinematic = true;
            m_BallInCharge.SetOwner(m_Actor.Transform);
            m_BallInCharge.SetTeslaFiring(this);
            m_BallInCharge.transform.SetParent(m_Actor.FirePoint);
            m_BallInCharge.transform.position = m_Actor.FirePoint.position;
            m_BallInCharge.transform.rotation = m_Actor.FirePoint.rotation;
            m_BallInCharge.Show();
        }

        private float CalculateProjectileInitialSpeed()
        {
            return Mathf.Lerp(
                m_Data.MinInitialSpeed,
                m_Data.MaxInitialSpeed,
                m_ChargeAmount) + m_Actor.CurrentMoveSpeed;
        }

        private void CalculateBallDamage(out int damage)
        {
            damage = (int)Mathf.Lerp(m_Data.MinDamage, m_Data.MaxDamage, m_ChargeAmount);
        }

        private void PlayChargingClip()
        {
            /*m_FiringAudioSource.clip = m_Data.ChargingClip;
            m_FiringAudioSource.Play();*/
        }

        // private void UpdateChargingClipPitch(float amount) => m_FiringAudioSource.pitch = 0.5f + amount;
        // private void StopChargingClip() => m_FiringAudioSource.Stop();
        // private void PlayShotFiredClip() => m_AudioPool.GetAudioView().PlayOneShot(m_Data.ShotFiredClip, m_Actor.Transform.position);

        /*private void InitializeFiringAudio()
        {
            if (m_AudioPool == null)
            {
                Debug.LogError("No Audio pool found!");
                return;
            }

            m_FiringAudioSource = m_AudioPool.GetAudioView().AudioSource;
            m_FiringAudioSource.gameObject.name = FIRING_AUDIO_SOURCE_NAME;
            m_FiringAudioSource.transform.SetParent(m_Actor.Transform, Vector3.zero, Quaternion.identity);
            m_FiringAudioSource.spatialBlend = 1f;
            m_FiringAudioSource.playOnAwake = false;
            m_FiringAudioSource.loop = false;
            m_FiringAudioSource.gameObject.SetActive(true);
        }*/

        // private void DeInitializeFiringAudio() => m_AudioPool.ReturnAudio(m_FiringAudioSource.GetComponent<AudioView>());
    }
}

