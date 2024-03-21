using UnityEngine;
using UnityEngine.InputSystem;

public class TankFiring
{
    // dependencies
    TankModel m_Model;
    InputControls m_InputControls;
    TankView m_View;
    TankProjectilePool m_ProjectilePool;

    public TankFiring(TankModel model, InputControls inputControls, TankView view)
    {
        m_Model = model;
        m_InputControls = inputControls;
        m_View = view;
        m_ProjectilePool = new TankProjectilePool(m_Model.TankData.ProjectileData);

        m_InputControls.Player.Fire.started += OnFireInputActionStarted;
        m_InputControls.Player.Fire.canceled += OnFireInputActionCanceled;
    }

    public void Update()
    {
        UpdateChargeAmount();
    }

    ~TankFiring()
    {
        m_InputControls.Player.Fire.started -= OnFireInputActionStarted;
        m_InputControls.Player.Fire.canceled -= OnFireInputActionCanceled;
    }

    private void OnFireInputActionStarted(InputAction.CallbackContext context)
    {
        m_Model.IsFiring = true;
    }

    private void OnFireInputActionCanceled(InputAction.CallbackContext context)
    {
        m_Model.IsFiring = false;
        SpawnProjectile();
        m_Model.ChargeAmount = 0f;
    }

    private void UpdateChargeAmount()
    {
        if (!m_Model.IsFiring)
            return;

        m_Model.ChargeAmount += Time.deltaTime / m_Model.TankData.ChargeTime;
        m_Model.ChargeAmount = Mathf.Clamp01(m_Model.ChargeAmount);
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
