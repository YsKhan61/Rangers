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

        // cache
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_View.transform;

        public Transform CameraTarget => m_Entity.CameraTarget;

        // cached values
        private float m_AccelerationMagnitude;
        private float m_RotateAngle;
        private Quaternion m_DeltaRotation;

        public PlayerTankController(PlayerService service, PlayerDataSO data)
        {
            m_PlayerService = service;
            m_Model = new PlayerModel(data);
            m_View = Object.Instantiate(data.Prefab);
        }

        ~PlayerTankController()
        {
            UnityCallbacks.Instance.Unregister(this as IFixedUpdatable);
            UnityCallbacks.Instance.Unregister(this as IUpdatable);
        }

        public void SetEntity(IEntityBrain entity)
        {
            m_Entity = entity as IEntityTankBrain;
            m_Model.EntityModel = m_Entity.Model;
            m_Model.IsEnabled = true;

            m_Entity.Model.IsPlayer = true;
            m_Entity.SetLayers(m_Model.PlayerData.SelfLayer, m_Model.PlayerData.OppositionLayer);
            m_Entity.SetParentOfView(Transform, Vector3.zero, Quaternion.identity);
            m_Entity.SetRigidbody(Rigidbody);
            m_Entity.OnEntityInitialized += m_PlayerService.OnEntityInitialized;
            m_Entity.PrimaryAction.OnPlayerCamShake += m_Model.PlayerData.OnCameraShake.RaiseEvent;
            m_Entity.OnPlayerCamShake += m_Model.PlayerData.OnCameraShake.RaiseEvent;
            m_Entity.HealthController.OnHealthUpdated += OnEntityHealthUpdated;
            m_Entity.UltimateAction.OnUltimateActionAssigned += m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_Entity.UltimateAction.OnChargeUpdated += m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_Entity.UltimateAction.OnFullyCharged += m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_Entity.UltimateAction.OnUltimateActionExecuted += m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;
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
            UnityCallbacks.Instance.Unregister(this as IFixedUpdatable);
            UnityCallbacks.Instance.Unregister(this as IUpdatable);

            m_Entity.OnEntityInitialized -= m_PlayerService.OnEntityInitialized;
            m_Entity.HealthController.OnHealthUpdated -= OnEntityHealthUpdated;
            m_Entity.PrimaryAction.OnPlayerCamShake -= m_Model.PlayerData.OnCameraShake.RaiseEvent;
            m_Entity.OnPlayerCamShake -= m_Model.PlayerData.OnCameraShake.RaiseEvent;
            m_Entity.UltimateAction.OnUltimateActionAssigned -= m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_Entity.UltimateAction.OnChargeUpdated -= m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_Entity.UltimateAction.OnFullyCharged -= m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_Entity.UltimateAction.OnUltimateActionExecuted -= m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;
            m_Entity.OnAfterDeath -= OnTankDead;

            m_Model.IsEnabled = false;
            m_Model.EntityModel = null;
            m_Entity = null;

            m_PlayerService.OnPlayerDeath();
        }

        private void OnEntityHealthUpdated(int currentHealth, int maxHealth)
        {
            m_Model.PlayerData.OnPlayerHealthUpdated.RaiseEvent(currentHealth, maxHealth);
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
