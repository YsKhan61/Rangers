using BTG.Tank;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Player
{
    public class PlayerController : IFixedUpdatable, IUpdatable
    {
        private PlayerModel m_Model;

        private TankMainController m_TankController;

        // cache
        private Rigidbody TankRigidbody => m_TankController.Rigidbody;
        private Transform TankTransform => m_TankController.Transform;

        // cached values
        private float m_AccelerationMagnitude;
        private float m_RotateAngle;
        private Quaternion m_DeltaRotation;

        public PlayerController()
        {
            m_Model = new PlayerModel();
        }

        ~PlayerController()
        {
            UnityCallbacks.Instance.Deregister(this as IFixedUpdatable);
            UnityCallbacks.Instance.Unregister(this as IUpdatable);
        }

        public void SetTank(TankMainController tankController)
        {
            m_TankController = tankController;
            m_Model.TankModel = tankController.Model;

            m_Model.IsEnabled = true;

            UnityCallbacks.Instance.Register(this as IFixedUpdatable);
            UnityCallbacks.Instance.Register(this as IUpdatable);
        }

        public void SetMoveValue(float value)
        {
            if (!m_Model.IsEnabled)
                return;

            m_Model.MoveInputValue = value;
        }

        public void SetRotateValue(float value)
        {
            if (!m_Model.IsEnabled)
                return;

            m_Model.RotateInputValue = value;
        }

        public void StartFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_TankController?.FiringController.OnFireStarted();
        }

        public void StopFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_TankController?.FiringController.OnFireStopped();
        }

        public void TryExecuteUltimate()
        {
            if (!m_Model.IsEnabled)
                return;

            m_TankController?.UltimateController.UltimateAction.TryExecute();
        }

        public void FixedUpdate()
        {
            if (!m_Model.IsEnabled)
                return;

            MoveWithForce();
        }

        public void Update()
        {
            if (!m_Model.IsEnabled)
                return;

            Rotate();
            CalculateInputSpeed();
        }

        public void OnTankDead()
        {
            UnityCallbacks.Instance.Deregister(this as IFixedUpdatable);
            UnityCallbacks.Instance.Unregister(this as IUpdatable);

            m_Model.IsEnabled = false;
            m_Model.TankModel = null;
            m_TankController = null;
        }

        private void MoveWithForce()
        {
            TankRigidbody.AddForce(TankTransform.forward * m_AccelerationMagnitude, ForceMode.Acceleration);
            TankRigidbody.velocity = Vector3.ClampMagnitude(TankRigidbody.velocity, m_Model.TankMaxSpeed);
        }

        private void Rotate()
        {
            m_RotateAngle = m_Model.TankRotateSpeed * m_Model.RotateInputValue * Time.deltaTime *
                (m_Model.MoveInputValue > 0 ? 1 :
                    (m_Model.MoveInputValue < 0 ? -1 : 0)
                    );

            m_DeltaRotation = Quaternion.Euler(0, m_RotateAngle, 0);
            TankRigidbody.MoveRotation(TankRigidbody.rotation * m_DeltaRotation);
        }

        private void CalculateInputSpeed()
        {
            m_AccelerationMagnitude = m_Model.TankAcceleration * m_Model.MoveInputValue;
        }
    }
}
