using UnityEngine;
using UnityEngine.InputSystem;

public class TankController
{
    // dependencies
    private TankModel m_TankModel;
    private TankView m_TankView;
    private TankProjectilePool m_ProjectilePool;
    private InputControls m_InputControls;
    private Rigidbody m_Rigidbody;

    public Transform Transform => m_TankView.transform;

    // cache
    private InputAction m_MoveInputAction;
    private InputAction m_RotateInputAction;
    private float m_MoveInputValue;
    private Vector3 m_DeltaPosition;
    private float m_RotateAngle;
    private Quaternion m_DeltaRotation;


    public TankController(TankDataSO tankData)
    {
        m_TankModel = new TankModel(tankData, this);
        m_TankView = Object.Instantiate(tankData.TankViewPrefab);
        m_TankView.SetController(this);

        m_ProjectilePool = new TankProjectilePool(tankData.ProjectileData);

        m_Rigidbody = m_TankView.RigidBody;

        ConfigureInputs();
    }

    ~TankController()
    {
        m_InputControls.Player.Fire.started -= OnFireInputActionStarted;
        m_InputControls.Player.Fire.canceled -= OnFireInputActionCanceled;
        m_InputControls.Player.Disable();
    }

    public void Update()
    {
        Move();
        Fire();
    }

    private void ConfigureInputs()
    {
        m_InputControls = new InputControls();
        m_InputControls.Enable();
        m_InputControls.Player.Enable();

        m_MoveInputAction = m_InputControls.Player.MoveAction;
        m_RotateInputAction = m_InputControls.Player.RotateAction;
        m_InputControls.Player.Fire.started += OnFireInputActionStarted;
        m_InputControls.Player.Fire.canceled += OnFireInputActionCanceled;
    }

    private void OnFireInputActionStarted(InputAction.CallbackContext context)
    {
        m_TankModel.IsFiring = true;
    }

    private void OnFireInputActionCanceled(InputAction.CallbackContext context)
    {
        m_TankModel.IsFiring = false;
    }

    private void Move()
    {
        m_MoveInputValue = m_MoveInputAction.ReadValue<float>();
        m_DeltaPosition = m_Rigidbody.position + m_TankView.transform.forward * m_TankModel.TankData.MoveSpeed * m_MoveInputValue * Time.deltaTime;
        m_RotateAngle = m_TankModel.TankData.RotateSpeed * m_RotateInputAction.ReadValue<float>() * Time.deltaTime * (m_MoveInputValue > 0 ? 1 : (m_MoveInputValue < 0 ? - 1 : 0));
        m_DeltaRotation = m_Rigidbody.rotation * Quaternion.Euler(0, m_RotateAngle, 0);
        m_Rigidbody.Move(m_DeltaPosition, m_DeltaRotation);
    }

    private void Fire()
    {
        if (!m_TankModel.IsFiring)
            return;

        if (Time.time - m_TankModel.LastFireTime < m_TankModel.TankData.FireRate)
            return;

        TankProjectileController projectileController = m_ProjectilePool.GetProjectile();
        projectileController.Transform.position = m_TankView.FirePoint.position;
        projectileController.Transform.rotation = m_TankView.FirePoint.rotation;
        projectileController.AddImpulseForce();

        m_TankModel.LastFireTime = Time.time;
    }
}
