using UnityEngine;
using UnityEngine.InputSystem;

public class TankController
{
    // dependencies
    private TankModel m_TankModel;
    private TankView m_TankView;
    private InputControls m_inputControls;
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

        m_Rigidbody = m_TankView.RigidBody;

        ConfigureInputs();
    }

    ~TankController()
    {
        m_inputControls.Player.Disable();
    }

    private void ConfigureInputs()
    {
        m_inputControls = new InputControls();
        m_inputControls.Enable();
        m_inputControls.Player.Enable();

        m_MoveInputAction = m_inputControls.Player.MoveAction;
        m_RotateInputAction = m_inputControls.Player.RotateAction;
    }

    public void Move()
    {
        m_MoveInputValue = m_MoveInputAction.ReadValue<float>();
        m_DeltaPosition = m_Rigidbody.position + m_TankView.transform.forward * m_TankModel.TankData.MoveSpeed * m_MoveInputValue * Time.deltaTime;
        m_RotateAngle = m_TankModel.TankData.RotateSpeed * m_RotateInputAction.ReadValue<float>() * Time.deltaTime * (m_MoveInputValue > 0 ? 1 : (m_MoveInputValue < 0 ? - 1 : 0));
        m_DeltaRotation = m_Rigidbody.rotation * Quaternion.Euler(0, m_RotateAngle, 0);
        m_Rigidbody.Move(m_DeltaPosition, m_DeltaRotation);
    }
}
