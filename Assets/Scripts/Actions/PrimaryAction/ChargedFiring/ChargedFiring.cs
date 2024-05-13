using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// Firing happens by charging the projectile and releasing it.
    /// </summary>
    public class ChargedFiring : IPrimaryAction, IUpdatable, IDestroyable
    {
        private const string FIRING_AUDIO_SOURCE_NAME = "FiringAudioSource";

        private ChargedFiringDataSO m_Data;

        public event Action<float, float> OnPlayerCamShake;

        private IPrimaryActor m_Actor;
        private ProjectilePool m_Pool;

        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;

        private readonly AudioSource m_FiringAudioSource;
        

        public ChargedFiring(ChargedFiringDataSO data, IPrimaryActor actor, ProjectilePool pool)
        {
            m_Data = data;
            m_Actor = actor;
            m_Pool = pool;
            m_FiringAudioSource = new GameObject(FIRING_AUDIO_SOURCE_NAME).AddComponent<AudioSource>();
            ConfigureAudioSource();
        }

        public void Enable()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this as IUpdatable);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this as IDestroyable);
            ToggleMuteFiringAudio(false);

            // Add all the event listeners of OnTankShoot
            m_IsEnabled = true;
        }

        public void Update()
        {
            if (!m_IsEnabled)
                return;

            UpdateChargeAmount();
        }

        public void Disable()
        {
            ResetCharging();
            ToggleMuteFiringAudio(true);
            m_IsEnabled = false;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this as IUpdatable);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this as IDestroyable);
        }

        public void Destroy()
        {
            // Remove all the event listeners of OnTankShoot
            OnPlayerCamShake = null;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this as IUpdatable);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this as IDestroyable);
        }

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;
            PlayChargingClip();
        }

        public void StopAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = false;

            if (m_ChargeAmount <= 0f)
                return;

            SpawnProjectile(out ProjectileController projectile);
            projectile.AddImpulseForce(CalculateProjectileInitialSpeed());

            if (m_Actor.IsPlayer)
                OnPlayerCamShake?.Invoke(m_ChargeAmount, 0.5f);

            PlayShotFiredClip();
            ResetCharging();
        }

        private void UpdateChargeAmount()
        {
            if (!m_IsCharging)
                return;

            m_ChargeAmount += Time.deltaTime / m_Data.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);
            // m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            UpdateChargingClipPitch(m_ChargeAmount);
        }

        private void ResetCharging()
        {
            m_ChargeAmount = 0f;
            // m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            StopChargingClip();
        }

        private void SpawnProjectile(out ProjectileController projectile)
        {
            projectile = m_Pool.GetProjectile();
            projectile.Init();
            projectile.Transform.position = m_Actor.FirePoint.position;
            projectile.Transform.rotation = m_Actor.FirePoint.rotation;
        }

        private float CalculateProjectileInitialSpeed()
        {
            return Mathf.Lerp(
                m_Data.MinInitialSpeed,
                m_Data.MaxInitialSpeed,
                m_ChargeAmount) + m_Actor.CurrentMoveSpeed;
        }

        private void ConfigureAudioSource()
        {
            m_FiringAudioSource.transform.SetParent(m_Actor.Transform);
            m_FiringAudioSource.spatialBlend = 1f;
            m_FiringAudioSource.playOnAwake = false;
            m_FiringAudioSource.loop = false;
        }

        private void ToggleMuteFiringAudio(bool mute) => m_FiringAudioSource.mute = mute;
        private void PlayChargingClip()
        {
            m_FiringAudioSource.clip = m_Data.ChargeClip;
            m_FiringAudioSource.Play();
        }

        private void UpdateChargingClipPitch(float amount) => m_FiringAudioSource.pitch = 0.5f + amount;
        private void StopChargingClip() => m_FiringAudioSource.Stop();
        private void PlayShotFiredClip()
        {
            m_FiringAudioSource.pitch = 1f;
            m_FiringAudioSource.PlayOneShot(m_Data.ShotFiredClip);
        }
    }

}
