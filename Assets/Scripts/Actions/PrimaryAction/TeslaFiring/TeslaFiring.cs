using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    public class TeslaFiring : IPrimaryAction, IUpdatable
    {
        private TeslaFiringDataSO m_Data;
        private IPrimaryActor m_Actor;
        private TeslaBallPool m_Pool;

        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;

        private TeslaBallView m_BallInCharge;

        public TeslaFiring(TeslaFiringDataSO data, IPrimaryActor actor, TeslaBallPool pool)
        {
            m_Data = data;
            m_Actor = actor;
            m_Pool = pool;
        }

        public void Enable()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);

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
            m_IsEnabled = false;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
        }

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;

            SpawnBall();
        }

        public void StopAction()
        {
            if (!m_IsEnabled || !m_IsCharging || m_BallInCharge == null)
                return;

            m_IsCharging = false;

            CalculateBallDamage(out int damage);
            m_BallInCharge?.SetDamage(damage);
            m_BallInCharge.AddImpulseForce(CalculateProjectileInitialSpeed());

            if (m_Actor.IsPlayer)
                EventBus<CameraShakeEvent>.Invoke(new CameraShakeEvent { ShakeAmount = m_ChargeAmount, ShakeDuration = 0.5f });  // OnPlayerCamShake?.Invoke(m_ChargeAmount, 0.5f);
                
            ResetCharging();
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

            m_ChargeAmount += Time.deltaTime / m_Data.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);   
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
            m_BallInCharge = m_Pool.GetTeslaBall();
            m_BallInCharge.Init();
            m_BallInCharge.transform.position = m_Actor.FirePoint.position;
            m_BallInCharge.transform.rotation = m_Actor.FirePoint.rotation;
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
    }
}

