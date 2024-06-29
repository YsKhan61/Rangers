using BTG.AudioSystem;
using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System;
using System.Threading;
using UnityEngine;
using VContainer;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// Firing happens by charging the projectile and releasing it.
    /// </summary>
    public class ChargedFiring : IPrimaryAction, IUpdatable, IDestroyable
    {
        private const string FIRING_AUDIO_SOURCE_NAME = "FiringAudioSource";

        public event Action OnPrimaryActionExecuted;

        /*[Inject]
        private AudioPool m_AudioPool;*/

        private IPrimaryActor m_Actor;
        private ProjectilePool m_ProjectilePool;
        private ChargedFiringDataSO m_Data;
        private AudioSource m_FiringAudioSource;
        private CancellationTokenSource m_Cts;

        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;

        public ChargedFiring(ChargedFiringDataSO data, ProjectilePool projectilePool)
        {
            m_Data = data;
            m_ProjectilePool = projectilePool;

            // CreateFiringAudio();
        }

        public void Enable()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
            // InitializeFiringAudio();

            m_IsEnabled = true;
        }

        public void Update()
        {
            if (!m_IsEnabled)
                return;

            UpdateChargeAmount();

            ShootOnFullyCharged();
        }

        public void Disable()
        {
            ResetCharging();
            // DeInitializeFiringAudio();
            m_IsEnabled = false;
            m_Cts?.Cancel();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public void Destroy()
        {
            m_ProjectilePool.ClearPool();
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public void SetActor(IPrimaryActor actor) => m_Actor = actor;

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;
            // PlayChargingClip();
        }

        public void StopAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = false;

            if (m_ChargeAmount <= 0f)
                return;

            SpawnProjectileAndShoot();

            if (m_Actor.IsPlayer)
                m_Actor.RaisePlayerCamShakeEvent(new CameraShakeEventData { ShakeAmount = m_ChargeAmount, ShakeDuration = 0.5f });

            OnPrimaryActionExecuted?.Invoke();

            // PlayShotFiredClip();
            ResetCharging();
        }

        public void AutoStartStopAction(int stopTime)
        {
            StartAction();

            m_Cts = new CancellationTokenSource();

            _ = HelperMethods.InvokeAfterAsync(stopTime , () =>
            {
                StopAction();
            }, m_Cts.Token);
        }

        private void UpdateChargeAmount()
        {
            if (!m_IsCharging)
                return;

            m_ChargeAmount += Time.deltaTime / m_Data.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);
            // UpdateChargingClipPitch(m_ChargeAmount);
        }

        private void ShootOnFullyCharged()
        {
            if (m_ChargeAmount >= 1f)
            {
                StopAction();
            }
        }

        private void SpawnProjectileAndShoot()
        {
            SpawnProjectile(out ProjectileController projectile);
            projectile.AddImpulseForce(CalculateProjectileInitialSpeed());
        }

        private void ResetCharging()
        {
            m_ChargeAmount = 0f;
            // StopChargingClip();
        }

        private void SpawnProjectile(out ProjectileController projectile)
        {
            projectile = CreateProjectile();
            projectile.SetPositionAndRotation(m_Actor.FirePoint.position, m_Actor.FirePoint.rotation);
            projectile.SetOwnerOfView(m_Actor.Transform);
            projectile.SetActor(m_Actor);
            projectile.Init();
            projectile.ShowView();
        }

        private ProjectileController CreateProjectile()
        {
            ProjectileView view = m_ProjectilePool.GetProjectile();
            ProjectileController pc = new ProjectileController(m_Data, view);
            view.SetController(pc);
            // pc.SetAudioPool(m_AudioPool);
            return pc;
        }

        private float CalculateProjectileInitialSpeed()
        {
            return Mathf.Lerp(
                m_Data.MinInitialSpeed,
                m_Data.MaxInitialSpeed,
                m_ChargeAmount) + m_Actor.CurrentMoveSpeed;
        }

        private void PlayChargingClip()
        {
            m_FiringAudioSource.clip = m_Data.ChargeClip;
            m_FiringAudioSource.Play();
        }

        /*private void UpdateChargingClipPitch(float amount) => m_FiringAudioSource.pitch = 0.5f + amount;
        private void StopChargingClip() => m_FiringAudioSource.Stop();
        // private void PlayShotFiredClip() => m_AudioPool.GetAudioView().PlayOneShot(m_Data.ShotFiredClip, m_Actor.Transform.position);
        private void PlayShotFiredClip() => m_FiringAudioSource.PlayOneShot(m_Data.ShotFiredClip);

        private void CreateFiringAudio()
        {
            *//*if (m_AudioPool == null)
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
            m_FiringAudioSource.gameObject.SetActive(true);*//*

            m_FiringAudioSource = new GameObject(FIRING_AUDIO_SOURCE_NAME).AddComponent<AudioSource>();
            m_FiringAudioSource.transform.SetParent(m_Actor.Transform, Vector3.zero, Quaternion.identity);
            m_FiringAudioSource.spatialBlend = 1f;
            m_FiringAudioSource.playOnAwake = false;
            m_FiringAudioSource.loop = false;
            m_FiringAudioSource.gameObject.SetActive(true);
        }

        private void DeInitializeFiringAudio() => m_AudioPool.ReturnAudio(m_FiringAudioSource.GetComponent<AudioView>());*/
    }


}
