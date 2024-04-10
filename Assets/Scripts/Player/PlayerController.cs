using BTG.Entity;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Player
{
    public class PlayerController : IFixedUpdatable, IUpdatable
    {
        private PlayerService m_PlayerService;
        private PlayerModel m_Model;
        private PlayerView m_View;

        private IEntityBrain m_Entity;

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

        public void SetEntity(IEntityBrain entity)
        {
            m_Entity = entity;
            m_Model.EntityModel = entity.Model;
            m_Model.IsEnabled = true;

            m_Entity.Model.IsPlayer = true;
            m_Entity.SetLayers(m_Model.PlayerData.SelfLayer, m_Model.PlayerData.OppositionLayer);
            m_Entity.SetParentOfView(Transform, Vector3.zero, Quaternion.identity);
            m_Entity.SetRigidbody(Rigidbody);
            m_Entity.OnEntityInitialized += OnEntityInitialized;
            m_Entity.HealthController.OnHealthUpdated += OnEntityHealthUpdated;
            m_Entity.OnAfterDeath += OnTankDead;
            m_Entity.Init();

            Rigidbody.centerOfMass = m_Entity.Transform.position;
            Rigidbody.maxLinearVelocity = m_Model.EntityMaxSpeed;

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

            m_Entity?.StartFire();
        }

        public void StopFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_Entity?.StopFire();
        }

        public void TryExecuteUltimate()
        {
            if (!m_Model.IsEnabled)
                return;

            m_Entity?.TryExecuteUltimate();
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

            m_Entity.OnEntityInitialized -= OnEntityInitialized;
            m_Entity.HealthController.OnHealthUpdated -= OnEntityHealthUpdated;
            m_Entity.OnAfterDeath -= OnTankDead;

            m_Model.IsEnabled = false;
            m_Model.EntityModel = null;
            m_Entity = null;

            m_PlayerService.OnPlayerDeath();
        }

        private void OnEntityInitialized(Sprite sprite)
        {
            m_Model.PlayerData.PlayerIcon.Value = sprite;
        }

        private void OnEntityHealthUpdated(int currentHealth, int maxHealth)
        {
            m_Model.PlayerData.PlayerHealthEventChannel.RaiseEvent(currentHealth, maxHealth);
        }

        private void MoveWithForce()
        {
            Rigidbody.AddForce(Transform.forward * m_AccelerationMagnitude, ForceMode.Acceleration);
            Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, m_Model.EntityMaxSpeed);
        }

        private void Rotate()
        {
            m_RotateAngle = m_Model.EntityRotateSpeed * m_Model.RotateInputValue * Time.fixedDeltaTime *
                (m_Model.MoveInputValue > 0 ? 1 :
                    (m_Model.MoveInputValue < 0 ? -1 : 0)
                    );

            m_DeltaRotation = Quaternion.Euler(0, m_RotateAngle, 0);
            Rigidbody.MoveRotation(Rigidbody.rotation * m_DeltaRotation);
        }

        private void CalculateInputSpeed()
        {
            m_AccelerationMagnitude = m_Model.EntityAcceleration * m_Model.MoveInputValue;
        }
    }
}
