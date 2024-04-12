using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// Firing happens by charging the projectile and releasing it.
    /// </summary>
    public class ChargedFiring : IPrimaryAction
    {
        private ChargedFiringDataSO m_Data;

        public event Action<float, float> OnPlayerCamShake;

        private IPrimaryActor m_Actor;
        private ProjectilePool m_Pool;

        private bool m_IsEnabled;
        private bool m_IsCharging;
        private float m_ChargeAmount;
        

        public ChargedFiring(ChargedFiringDataSO data, IPrimaryActor actor, ProjectilePool pool)
        {
            m_Data = data;
            m_Actor = actor;
            m_Pool = pool;
        }

        public void Enable()
        {
            UnityCallbacks.Instance.Register(this as IUpdatable);
            UnityCallbacks.Instance.Register(this as IDestroyable);

            // Add all the event listeners of OnTankShoot
            m_IsEnabled = true;
        }

        public void Update()
        {
            UpdateChargeAmount();
        }

        public void Disable()
        {
            ResetChargedAmount();
            m_IsEnabled = false;

            UnityCallbacks.Instance.Unregister(this as IUpdatable);
            UnityCallbacks.Instance.Unregister(this as IDestroyable);
        }

        public void OnDestroy()
        {
            // Remove all the event listeners of OnTankShoot

            UnityCallbacks.Instance.Unregister(this as IUpdatable);
            UnityCallbacks.Instance.Unregister(this as IDestroyable);
        }

        public void StartAction()
        {
            if (!m_IsEnabled)
                return;

            m_IsCharging = true;
            // m_View.AudioView.PlayChargingClip(m_Model.TankData.ShotChargingClip);
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

            // m_View.AudioView.PlayShotFiringClip(m_Model.TankData.ShotFiringClip);
            ResetChargedAmount();
        }

        private void UpdateChargeAmount()
        {
            if (!m_IsEnabled || !m_IsCharging)
                return;

            m_ChargeAmount += Time.deltaTime / m_Data.ChargeTime;
            m_ChargeAmount = Mathf.Clamp01(m_ChargeAmount);
            // m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            // m_View.AudioView.UpdateChargingClipPitch(m_Model.ChargeAmount);
        }

        private void ResetChargedAmount()
        {
            m_ChargeAmount = 0f;
            // m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            // m_View.AudioView.StopChargingClip();
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
    }

}
