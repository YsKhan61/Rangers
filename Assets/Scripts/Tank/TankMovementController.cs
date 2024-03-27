using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// This controller controls the movement of tank
    /// </summary>
    public class TankMovementController
    {

        // dependencies
        private TankModel m_TankModel;
        private TankMainController m_Controller;

        private Rigidbody m_Rigidbody;

        // cached values
        private float m_AccelerationMagnitude;
        private float m_RotateAngle;
        private Quaternion m_DeltaRotation;

        public TankMovementController(TankMainController controller)
        {
            m_Controller = controller;
            m_TankModel = controller.Model;
            m_Rigidbody = controller.Rigidbody;
        }

        public void FixedUpdate()
        {
            MoveWithForce();
        }

        public void Update()
        {
            Rotate();
            CalculateInputSpeed();
        }

        private void MoveWithForce()
        {
            m_Rigidbody.AddForce(m_Controller.Transform.forward * m_AccelerationMagnitude, ForceMode.Acceleration);
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_TankModel.TankData.MaxSpeed);
        }

        private void Rotate()
        {
            m_RotateAngle = m_TankModel.TankData.RotateSpeed * m_TankModel.RotateInputValue *
                Time.deltaTime *
                (m_TankModel.MoveInputValue > 0 ? 1 :
                    (m_TankModel.MoveInputValue < 0 ? -1 : 0)
                        );

            m_DeltaRotation = Quaternion.Euler(0, m_RotateAngle, 0);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * m_DeltaRotation);
        }

        private void CalculateInputSpeed()
        {
            m_AccelerationMagnitude = m_TankModel.TankData.Acceleration * m_TankModel.MoveInputValue;
        }
    }
}

