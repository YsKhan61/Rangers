using BTG.Entity;
using BTG.Tank.Projectile;
using System;
using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// Firing happens by charging the projectile and releasing it.
    /// </summary>
    public class TankChargedFiringController : IEntityFiringController
    {
        public event Action<float, float> OnPlayerCamShake;

        // dependencies
        TankModel m_Model;
        IEntityBrain m_Brain;
        TankView m_View;
        ProjectilePool m_ProjectilePool;

        public TankChargedFiringController(TankModel model, IEntityBrain brain, TankView view)
        {
            m_Model = model;
            m_Brain = brain;
            m_View = view;
            m_ProjectilePool = new ProjectilePool(m_Model.TankData.ProjectileData);
        }

        public void Update()
        {
            UpdateChargeAmount();
        }

        public void OnDestroy()
        {
            // Remove all the event listeners of OnTankShoot
            
        }

        public void OnFireStarted()
        {
            m_Model.IsCharging = true;
            m_View.AudioView.PlayChargingClip(m_Model.TankData.ShotChargingClip);
        }

        public void OnFireStopped()
        {
            m_Model.IsCharging = false;

            if (m_Model.ChargeAmount <= 0f)
                return;

            SpawnProjectile(out ProjectileController projectile);
            projectile.AddImpulseForce(CalculateProjectileInitialSpeed());

            if (m_Model.IsPlayer)
                OnPlayerCamShake?.Invoke(m_Model.ChargeAmount, 0.5f);

            m_View.AudioView.PlayShotFiringClip(m_Model.TankData.ShotFiringClip);
            ResetChargedAmount();
        }

        private void UpdateChargeAmount()
        {
            if (!m_Model.IsCharging)
                return;

            m_Model.ChargeAmount += Time.deltaTime / m_Model.TankData.ChargeTime;
            m_Model.ChargeAmount = Mathf.Clamp01(m_Model.ChargeAmount);
            m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            m_View.AudioView.UpdateChargingClipPitch(m_Model.ChargeAmount);
        }

        private void ResetChargedAmount()
        {
            m_Model.ChargeAmount = 0f;
            m_View.UpdateChargedAmountUI(m_Model.ChargeAmount);
            m_View.AudioView.StopChargingClip();
        }

        private void SpawnProjectile(out ProjectileController projectile)
        {
            projectile = m_ProjectilePool.GetProjectile();
            projectile.Transform.position = m_View.FirePoint.position;
            projectile.Transform.rotation = m_View.FirePoint.rotation;
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
