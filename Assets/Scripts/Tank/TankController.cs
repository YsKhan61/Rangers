using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// The main controller for the tank. It handles the communications between Model, View and other controllers such as TankFiring and TankUltimateController.
    /// </summary>
    public class TankController
    {
        public enum TankState
        {
            Idle,
            Driving
        }

        // dependencies
        private TankModel m_Model;
        public TankModel TankModel => m_Model;
        private TankView m_View;
        public TankView TankView => m_View;
        private TankMovementController m_MovementController;
        private TankChargedFiringController m_Firing;
        public TankChargedFiringController TankFiring => m_Firing;
        private TankUltimateController m_TankUltimateController;
        public TankUltimateController TankUltimateController => m_TankUltimateController;

        public Transform Transform => m_View.transform;
        public Rigidbody Rigidbody => m_View.RigidBody;

        public Transform CameraTarget => m_View.CameraTarget;

        // cached values
        private float m_AccelerationMagnitude;
        private float m_RotateAngle;
        private Quaternion m_DeltaRotation;

        public TankController(TankDataSO tankData)
        {
            m_Model = new TankModel(tankData, this);
            m_View = Object.Instantiate(tankData.TankViewPrefab);
            m_View.SetController(this);
            m_MovementController = new TankMovementController(this);
            m_Firing = new TankChargedFiringController(m_Model, m_View);
            m_TankUltimateController = new TankUltimateController(this, m_Model.TankData.UltimateActionFactory.CreateUltimateAction());

            m_Model.State = TankState.Idle;
            OnTankStateChangedToIdle();
        }


        public void FixedUpdate()
        {
            m_MovementController.FixedUpdate();
        }

        public void Update()
        {
            UpdateState();

            m_MovementController.Update();
            m_Firing.Update();

            UpdateMoveSound();
        }

        public void OnDestroy()
        {
            m_Firing.OnDestroy();
        }

        public void SetMoveValue(float value)
        {
            m_Model.MoveInputValue = value;
        }

        public void SetRotateValue(float value)
        {
            m_Model.RotateInputValue = value;
        }

        private void UpdateState()
        {
            switch (m_Model.State)
            {
                case TankState.Idle:
                    if (Rigidbody.velocity.sqrMagnitude > 0.05f)
                    {
                        m_Model.State = TankState.Driving;
                        OnTankStateChangedToDriving();
                    }
                    break;
                case TankState.Driving:
                    if (Rigidbody.velocity.sqrMagnitude <= 0.05f)
                    {
                        m_Model.State = TankState.Idle;
                        OnTankStateChangedToIdle();
                    }
                    break;
            }
        }

        private void UpdateMoveSound()
        {
            if (m_Model.State == TankState.Driving)
            {
                m_View.TankAudio.UpdateEngineDrivingClipPitch(
                    Mathf.Lerp(0, 1, Mathf.InverseLerp(0, m_Model.TankData.MaxSpeed, m_Model.CurrentMoveSpeed)));
            }
        }

        private void OnTankStateChangedToIdle()
        {
            m_View.TankAudio.PlayEngineIdleClip(m_Model.TankData.EngineIdleClip);
        }

        private void OnTankStateChangedToDriving()
        {
            m_View.TankAudio.PlayEngineDrivingClip(m_Model.TankData.EngineDrivingClip);
        }
    }
}

