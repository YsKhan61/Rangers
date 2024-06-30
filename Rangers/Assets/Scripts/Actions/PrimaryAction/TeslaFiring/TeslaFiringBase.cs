using BTG.Events;
using BTG.Utilities;
using System.Threading;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public abstract class TeslaFiringBase : ITeslaFiring
    {
        private const string FIRING_AUDIO_SOURCE_NAME = "FiringAudioSource";

        public event System.Action<TagSO> OnActionAssigned;
        public event System.Action OnActionStarted;
        public event System.Action<float> OnActionChargeUpdated;
        public event System.Action OnActionExecuted;

        protected TeslaFiringDataSO teslaFringData;
        public TeslaFiringDataSO Data => teslaFringData;

        protected IPrimaryActor actor;
        protected ITeslaBallView m_BallInCharge;

        private CancellationTokenSource m_Cts;

        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;



        public TeslaFiringBase(TeslaFiringDataSO data)
        {
            teslaFringData = data;
        }

        public void Enable()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
            m_IsEnabled = true;
            OnActionAssigned?.Invoke(teslaFringData.Tag);
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

        public virtual void Disable()
        {
            m_IsEnabled = false;
            m_Cts?.Cancel();
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
        }

        public void SetActor(IPrimaryActor actor) => this.actor = actor;

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;
            SpawnBall();
            ConfigureBall();
            OnActionStarted?.Invoke();
        }

        public void StopAction()
        {
            if (!m_IsEnabled || !m_IsCharging || m_BallInCharge == null)
                return;

            m_IsCharging = false;
            SetDamageToBallAndShoot();
            OnActionExecuted?.Invoke();
            InvokeCameraShake();
            InvokeShootAudioEvent();
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

        protected abstract void SpawnBall();

        private void ConfigureBall()
        {
            if (m_BallInCharge == null)
                return;

            m_BallInCharge.Rigidbody.WakeUp();
            m_BallInCharge.Rigidbody.isKinematic = true;
            m_BallInCharge.SetOwner(actor.Transform);
            m_BallInCharge.Transform.SetPositionAndRotation(actor.FirePoint.position, actor.FirePoint.rotation);
            m_BallInCharge.Transform.localScale = Vector3.one * teslaFringData.MinTeslaBallScale;
            m_BallInCharge.Show();
        }



        protected abstract void InvokeShootAudioEvent();

        private void SetDamageToBallAndShoot()
        {
            m_BallInCharge.Rigidbody.isKinematic = false;
            CalculateBallDamage(out int damage);
            m_BallInCharge?.SetDamage(damage);
            m_BallInCharge.AddImpulseForce(CalculateProjectileInitialSpeed());
        }

        private void ResetCharging()
        {
            m_ChargeAmount = 0;
            m_BallInCharge = null;
        }

        private void UpdateChargeAmount()
        {
            if (!m_IsCharging)
                return;

            m_ChargeAmount += Time.deltaTime / teslaFringData.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);
            m_BallInCharge.Transform.SetPositionAndRotation(actor.FirePoint.position, actor.FirePoint.rotation);
            OnActionChargeUpdated?.Invoke(m_ChargeAmount);
        }

        private void UpdateBallSize()
        {
            if (m_BallInCharge == null)
                return;

            m_BallInCharge.Transform.localScale = Vector3.one * Mathf.Lerp(teslaFringData.MinTeslaBallScale, teslaFringData.MaxTeslaBallScale, m_ChargeAmount);
            m_BallInCharge.Collider.radius = Mathf.Lerp(teslaFringData.MinTeslaBallScale, teslaFringData.MaxTeslaBallScale, m_ChargeAmount);
        }

        private void ShootOnFullyCharged()
        {
            if (m_ChargeAmount >= 1)
            {
                StopAction();
            }
        }

        private float CalculateProjectileInitialSpeed()
        {
            return Mathf.Lerp(
                teslaFringData.MinInitialSpeed,
                teslaFringData.MaxInitialSpeed,
                m_ChargeAmount) + actor.CurrentMoveSpeed;
        }

        private void CalculateBallDamage(out int damage)
        {
            damage = (int)Mathf.Lerp(teslaFringData.MinDamage, teslaFringData.MaxDamage, m_ChargeAmount);
        }

        private void InvokeCameraShake()
        {
            if (actor.IsPlayer)
                actor.RaisePlayerCamShakeEvent(new CameraShakeEventData { ShakeAmount = m_ChargeAmount, ShakeDuration = 0.5f });
        }
    }
}

