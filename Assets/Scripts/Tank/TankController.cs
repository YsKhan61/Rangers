using UnityEngine;
using UnityEngine.InputSystem;

public class TankController
{
    private enum TankState
    {
        Idle,
        Driving
    }

    // dependencies
    private TankModel m_TankModel;
    private TankView m_TankView;
    private TankChargedFiring m_TankFiring;
    public TankChargedFiring TankFiring => m_TankFiring;

    /*private InputControls m_InputControls;
    public InputControls InputControls => m_InputControls;*/

    private Rigidbody m_Rigidbody;
    public Rigidbody Rigidbody => m_Rigidbody;

    public Transform CameraTarget => m_TankView.CameraTarget;

    // cache
    /*private InputAction m_MoveInputAction;
    private InputAction m_RotateInputAction;*/

    public float MoveInputValue;
    public float RotateInputValue;

    private float m_AccelerationMagnitude;
    private float m_RotateAngle;
    private Quaternion m_DeltaRotation;
    private TankState m_State;

    public TankController(TankDataSO tankData)
    {
        m_TankModel = new TankModel(tankData, this);
        m_TankView = Object.Instantiate(tankData.TankViewPrefab);
        m_TankView.SetController(this);
        m_TankFiring = new TankChargedFiring(m_TankModel, m_TankView);
        m_Rigidbody = m_TankView.RigidBody;

        m_State = TankState.Idle;
        OnTankStateChangedToIdle();
    }


    public void FixedUpdate()
    {
        MoveWithForce();
    }

    public void Update()
    {
        UpdateState();

        Rotate();
        CalculateInputSpeed();
        m_TankFiring.Update();

        UpdateMoveSound();
    }

    public void OnDestroy()
    {
        m_TankFiring.OnDestroy();
    }

    private void UpdateState()
    {
        switch (m_State)
        {
            case TankState.Idle:
                if (Rigidbody.velocity.sqrMagnitude > 0.05f)
                {
                    m_State = TankState.Driving;
                    OnTankStateChangedToDriving();
                }
                break;
            case TankState.Driving:
                if (Rigidbody.velocity.sqrMagnitude <= 0.05f)
                {
                    m_State = TankState.Idle;
                    OnTankStateChangedToIdle();
                }
                break;
        }
    }

    private void MoveWithForce()
    {
        m_Rigidbody.AddForce(m_TankView.transform.forward * m_AccelerationMagnitude, ForceMode.Acceleration);
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TankModel.TankData.MaxSpeed);
    }

    private void Rotate()
    {
        m_RotateAngle = m_TankModel.TankData.RotateSpeed * RotateInputValue * 
            Time.deltaTime * 
            (MoveInputValue > 0 ? 1 : 
                (MoveInputValue < 0 ? -1 : 0)
                    );

        m_DeltaRotation = Quaternion.Euler(0, m_RotateAngle, 0);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * m_DeltaRotation);
    }

    private void CalculateInputSpeed()
    {
        // MoveInputValue = m_MoveInputAction.ReadValue<float>();
        m_AccelerationMagnitude = m_TankModel.TankData.Acceleration * MoveInputValue;
    }

    private void UpdateMoveSound()
    {
        if (m_State == TankState.Driving)
        {
            m_TankView.TankAudio.UpdateEngineDrivingClipPitch(
                Mathf.Lerp(0,1, Mathf.InverseLerp(0, m_TankModel.TankData.MaxSpeed, m_TankModel.CurrentMoveSpeed)));
        }
    }

    private void OnTankStateChangedToIdle()
    {
        m_TankView.TankAudio.PlayEngineIdleClip(m_TankModel.TankData.EngineIdleClip);
    }

    private void OnTankStateChangedToDriving()
    {
        m_TankView.TankAudio.PlayEngineDrivingClip(m_TankModel.TankData.EngineDrivingClip);
    }
}
