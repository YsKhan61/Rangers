using UnityEngine;
using UnityEngine.InputSystem;

public class TankController
{
    // dependencies
    private TankModel m_TankModel;
    private TankView m_TankView;
    private TankFiring m_TankFiring;

    private InputControls m_InputControls;
    public InputControls InputControls => m_InputControls;

    private Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;

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
        ConfigureInputs();

        m_TankModel = new TankModel(tankData, this);
        m_TankView = Object.Instantiate(tankData.TankViewPrefab);
        m_TankView.SetController(this);
        m_TankFiring = new TankFiring(m_TankModel, m_InputControls, m_TankView);
        m_Rigidbody = m_TankView.RigidBody;
    }

    ~TankController()
    {
        m_InputControls.Player.Disable();
    }

    public void Update()
    {
        Move();
        m_TankFiring.Update();
    }

    private void ConfigureInputs()
    {
        m_InputControls = new InputControls();
        m_InputControls.Enable();
        m_InputControls.Player.Enable();

        m_MoveInputAction = m_InputControls.Player.MoveAction;
        m_RotateInputAction = m_InputControls.Player.RotateAction;
        
    }

    private void Move()
    {
        m_MoveInputValue = m_MoveInputAction.ReadValue<float>();
        m_DeltaPosition = m_Rigidbody.position + m_TankView.transform.forward * m_TankModel.TankData.MoveSpeed * m_MoveInputValue * Time.deltaTime;
        m_RotateAngle = m_TankModel.TankData.RotateSpeed * m_RotateInputAction.ReadValue<float>() * Time.deltaTime * (m_MoveInputValue > 0 ? 1 : (m_MoveInputValue < 0 ? - 1 : 0));
        m_DeltaRotation = m_Rigidbody.rotation * Quaternion.Euler(0, m_RotateAngle, 0);
        m_Rigidbody.Move(m_DeltaPosition, m_DeltaRotation);
    }
}
