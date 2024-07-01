using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System.Threading;
using System;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// A base class for charged firing action.
    /// </summary>
    public abstract class ChargedFiringBase : IPrimaryAction, IUpdatable, IDestroyable
    {
        private const string FIRING_AUDIO_SOURCE_NAME = "FiringAudioSource";

        public event Action<TagSO> OnActionAssigned;
        public event Action OnActionStarted;
        public event Action<float> OnActionChargeUpdated;
        public event Action OnActionExecuted;

        protected ProjectileDataSO chargedFiringData;
        protected IPrimaryActor actor;

        private AudioSource m_FiringAudioSource;
        private CancellationTokenSource m_Cts;
        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;

        public ChargedFiringBase(ProjectileDataSO data)
        {
            chargedFiringData = data;
        }

        public void Enable()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToDestroy(this);
            m_IsEnabled = true;
            OnActionAssigned?.Invoke(chargedFiringData.Tag);
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
            m_IsEnabled = false;
            m_Cts?.Cancel();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public virtual void Destroy()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromDestroy(this);
        }

        public void SetActor(IPrimaryActor actor) => this.actor = actor;

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;
            OnActionStarted?.Invoke();
        }

        public void StopAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = false;

            if (m_ChargeAmount <= 0f)
                return;

            SpawnProjectileAndShoot();
            OnActionExecuted?.Invoke();
            InvokeCameraShake();
            InvokeShootAudioEvent();
            ResetCharging();
        }

        private void InvokeCameraShake()
        {
            if (actor.IsPlayer)
                actor.RaisePlayerCamShakeEvent(new CameraShakeEventData { ShakeAmount = m_ChargeAmount, ShakeDuration = 0.5f });
        }

        protected abstract void InvokeShootAudioEvent();

        public void AutoStartStopAction(int stopTime)
        {
            StartAction();

            m_Cts = new CancellationTokenSource();

            _ = HelperMethods.InvokeAfterAsync(stopTime, () =>
            {
                StopAction();
            }, m_Cts.Token);
        }

        private void UpdateChargeAmount()
        {
            if (!m_IsCharging)
                return;

            m_ChargeAmount += Time.deltaTime / chargedFiringData.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);
            OnActionChargeUpdated?.Invoke(m_ChargeAmount);
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
        }

        private void SpawnProjectile(out ProjectileController projectile)
        {
            projectile = CreateProjectile();
            projectile.SetPositionAndRotation(actor.FirePoint.position, actor.FirePoint.rotation);
            projectile.SetOwnerOfView(actor.Transform);
            projectile.SetActor(actor);
            projectile.Init();
            projectile.ShowView();
        }

        protected abstract ProjectileController CreateProjectile();

        private float CalculateProjectileInitialSpeed()
        {
            return Mathf.Lerp(
                chargedFiringData.MinInitialSpeed,
                chargedFiringData.MaxInitialSpeed,
                m_ChargeAmount) + actor.CurrentMoveSpeed;
        }

        private void PlayChargingClip()
        {
            m_FiringAudioSource.clip = chargedFiringData.ChargeClip;
            m_FiringAudioSource.Play();
        }
    }
}
