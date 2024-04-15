using BTG.Entity;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerTankController : IFixedUpdatable, IUpdatable
    {
        private PlayerService m_PlayerService;
        private PlayerModel m_Model;
        private PlayerView m_View;

        private IEntityTankBrain m_Entity;

        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_Transform;

        public Transform CameraTarget => m_Entity.CameraTarget;

        // cached values
        private Transform m_Transform;

        public PlayerTankController(PlayerService service, PlayerDataSO data)
        {
            m_PlayerService = service;
            m_Model = new PlayerModel(data);
            m_View = Object.Instantiate(data.Prefab);
            m_Transform = m_View.transform;
        }

        ~PlayerTankController()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this as IUpdatable);
        }

        public void ConfigureWithEntity(IEntityBrain entity)
        {
            void ConfigurePlayer()
            {
                m_Model.IsEnabled = true;
                m_Entity = entity as IEntityTankBrain;
                m_Model.EntityMaxSpeed = m_Entity.Model.MaxSpeed;
                m_Model.EntityRotateSpeed = m_Entity.Model.RotateSpeed;
                m_Model.EntityAcceleration = m_Entity.Model.Acceleration;

                Rigidbody.centerOfMass = m_Entity.Transform.position;
                Rigidbody.maxLinearVelocity = m_Entity.Model.MaxSpeed;
            }
            ConfigurePlayer();
            
            void ConfigureEntity()
            {
                m_Entity.Model.IsPlayer = true;
                m_Entity.SetLayers(m_Model.PlayerData.SelfLayer, m_Model.PlayerData.OppositionLayer);
                m_Entity.SetParentOfView(Transform, Vector3.zero, Quaternion.identity);
                m_Entity.SetRigidbody(Rigidbody);
                m_Entity.OnEntityInitialized += m_PlayerService.OnEntityInitialized;
                m_Entity.HealthController.OnHealthUpdated += OnEntityHealthUpdated;
                m_Entity.UltimateAction.OnUltimateActionAssigned += m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
                m_Entity.UltimateAction.OnChargeUpdated += m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
                m_Entity.UltimateAction.OnFullyCharged += m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
                m_Entity.UltimateAction.OnUltimateActionExecuted += m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;
                m_Entity.OnAfterDeath += OnTankDead;
                m_Entity.Init();
            }
            ConfigureEntity();
            

            UnityMonoBehaviourCallbacks.Instance.RegisterToFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this as IUpdatable);
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

            m_Entity?.StartPrimaryFire();
        }

        public void StopFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_Entity?.StopPrimaryFire();
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
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this as IFixedUpdatable);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this as IUpdatable);

            m_Entity.OnEntityInitialized -= m_PlayerService.OnEntityInitialized;
            m_Entity.HealthController.OnHealthUpdated -= OnEntityHealthUpdated;
            m_Entity.UltimateAction.OnUltimateActionAssigned -= m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_Entity.UltimateAction.OnChargeUpdated -= m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_Entity.UltimateAction.OnFullyCharged -= m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_Entity.UltimateAction.OnUltimateActionExecuted -= m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;
            m_Entity.OnAfterDeath -= OnTankDead;

            m_Model.IsEnabled = false;
            m_Entity = null;

            m_PlayerService.OnPlayerDeath();
        }

        private void OnEntityHealthUpdated(int currentHealth, int maxHealth)
        {
            m_Model.PlayerData.OnPlayerHealthUpdated.RaiseEvent(currentHealth, maxHealth);
        }

        private void MoveWithForce()
        {
            Rigidbody.AddForce(Transform.forward * m_Model.Acceleration, ForceMode.Acceleration);
            Rigidbody.velocity = Vector3.ClampMagnitude(Rigidbody.velocity, m_Model.EntityMaxSpeed);
        }

        private void Rotate()
        {
            m_Model.RotateAngle = m_Model.EntityRotateSpeed * m_Model.RotateInputValue * Time.fixedDeltaTime *
                (m_Model.MoveInputValue > 0 ? 1 :
                    (m_Model.MoveInputValue < 0 ? -1 : 0)
                    );

            m_Model.DeltaRotation = Quaternion.Euler(0, m_Model.RotateAngle, 0);
            Rigidbody.MoveRotation(Rigidbody.rotation * m_Model.DeltaRotation);
        }

        private void CalculateInputSpeed()
        {
            m_Model.Acceleration = m_Model.EntityAcceleration * m_Model.MoveInputValue;
        }
    }
}
