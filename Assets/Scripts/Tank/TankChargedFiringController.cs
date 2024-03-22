using BTG.Tank.Projectile;
using System;
using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// Firing happens by charging the projectile and releasing it.
    /// </summary>
    public class TankChargedFiringController
    {
        // dependencies
        TankModel m_Model;
        TankView m_View;
        TankProjectilePool m_ProjectilePool;

        public event Action<float> OnTankShoot;

        public TankChargedFiringController(TankModel model, TankView view)
        {
            m_Model = model;
            m_View = view;
            m_ProjectilePool = new TankProjectilePool(m_Model.TankData.ProjectileData);
        }

        public void Update()
        {
            UpdateChargeAmount();
        }

        public void OnDestroy()
        {
            // Remove all the event listeners of OnTankShoot
            OnTankShoot = null;
        }

        public void OnFireStarted()
        {
            m_Model.IsCharging = true;
            m_View.TankAudio.PlayChargingClip(m_Model.TankData.ShotChargingClip);
        }

        public void OnFireEnded()
        {
            m_Model.IsCharging = false;
            SpawnProjectile();
            OnTankShoot?.Invoke(m_Model.ChargeAmount);
            m_View.TankAudio.PlayShotFiringClip(m_Model.TankData.ShotFiringClip);
            ResetChargedAmount();
        }

        private void UpdateChargeAmount()
        {
            if (!m_Model.IsCharging)
                return;

            m_Model.ChargeAmount += Time.deltaTime / m_Model.TankData.ChargeTime;
            m_Model.ChargeAmount = Mathf.Clamp01(m_Model.ChargeAmount);
            m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            m_View.TankAudio.UpdateChargingClipPitch(m_Model.ChargeAmount);
        }

        private void ResetChargedAmount()
        {
            m_Model.ChargeAmount = 0f;
            m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            m_View.TankAudio.StopChargingClip();
        }

        private void SpawnProjectile()
        {
            if (m_Model.ChargeAmount <= 0f)
                return;

            TankProjectileController projectileController = m_ProjectilePool.GetProjectile();
            projectileController.Transform.position = m_View.FirePoint.position;
            projectileController.Transform.rotation = m_View.FirePoint.rotation;
            projectileController.AddImpulseForce(CalculateProjectileInitialSpeed());
        }

        private float CalculateProjectileInitialSpeed()
        {
            return Mathf.Lerp(
                m_Model.TankData.ProjectileData.MinInitialSpeed,
                m_Model.TankData.ProjectileData.MaxInitialSpeed,
                m_Model.ChargeAmount) + m_Model.CurrentMoveSpeed;
        }
    }

}
