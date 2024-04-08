using BTG.Tank;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Player
{
    public class PlayerController : IFixedUpdatable, IUpdatable
    {
        private PlayerService m_PlayerService;
        private PlayerModel m_Model;
        private PlayerView m_View;

        private TankBrain m_Tank;

        // cache
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_View.transform;

        // cached values
        private float m_AccelerationMagnitude;
        private float m_RotateAngle;
        private Quaternion m_DeltaRotation;

        public PlayerController(PlayerService service, PlayerDataSO data)
        {
            m_PlayerService = service;
            m_Model = new PlayerModel(data);
            m_View = Object.Instantiate(data.Prefab);
        }

        ~PlayerController()
        {
            UnityCallbacks.Instance.Unregister(this as IFixedUpdatable);
            UnityCallbacks.Instance.Unregister(this as IUpdatable);
        }

        public void SetTank(TankBrain tank, int playerLayer, int enemyLayer)
        {
            m_Tank = tank;
            m_Model.TankModel = tank.Model;
            m_Model.IsEnabled = true;

            m_Tank.Model.IsPlayer = true;
            m_Tank.SetLayers(playerLayer, enemyLayer);
            m_Tank.SetParentOfView(Transform, Vector3.zero, Quaternion.identity);
            m_Tank.SetRigidbody(Rigidbody);
            m_Tank.OnAfterDeath += OnTankDead;
            m_Tank.Init();

            Rigidbody.centerOfMass = m_Tank.Transform.position;
            Rigidbody.maxLinearVelocity = m_Model.TankMaxSpeed;

            UnityCallbacks.Instance.Register(this as IFixedUpdatable);
            UnityCallbacks.Instance.Register(this as IUpdatable);
        }

        public void SetMoveValue(in float value)
        {
            if (!m_Model.IsEnabled)
                return;

            m_Model.MoveInputValue = value;
        }

        public void SetRotateValue(in float value)
        {
            if (!m_Model.IsEnabled)
                return;

            m_Model.RotateInputValue = value;
        }

        public void StartFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_Tank?.StartFire();
        }

        public void StopFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_Tank?.StopFire();
        }

        public void TryExecuteUltimate()
        {
            if (!m_Model.IsEnabled)
                return;

            m_Tank?.TryExecuteUltimate();
        }

        public void FixedUpdate()
        {
            if (!m_Model.IsEnabled)
                return;

            Rotate();

            MoveWithForce();
        }

        public void Update()
        {
            if (!m_Model.IsEnabled)
                return;

            CalculateInputSpeed();
        }

        private void OnTankDead()
        {
            UnityCallbacks.Instance.Unregister(this as IFixedUpdatable);
            UnityCallbacks.Instance.Unregister(this as IUpdatable);

            m_Tank.OnAfterDeath -= OnTankDead;

            m_Model.IsEnabled = false;
            m_Model.TankModel = null;
            m_Tank = null;

            m_PlayerService.OnPlayerDeath();
        }

        private void MoveWithForce()
        {
            Rigidbody.AddForce(Transform.forward * m_AccelerationMagnitude, ForceMode.Acceleration);
            Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, m_Model.TankMaxSpeed);
        }

        private void Rotate()
        {
            m_RotateAngle = m_Model.TankRotateSpeed * m_Model.RotateInputValue * Time.fixedDeltaTime *
                (m_Model.MoveInputValue > 0 ? 1 :
                    (m_Model.MoveInputValue < 0 ? -1 : 0)
                    );

            m_DeltaRotation = Quaternion.Euler(0, m_RotateAngle, 0);
            Rigidbody.MoveRotation(Rigidbody.rotation * m_DeltaRotation);
        }

        private void CalculateInputSpeed()
        {
            m_AccelerationMagnitude = m_Model.TankAcceleration * m_Model.MoveInputValue;
        }
    }
}
