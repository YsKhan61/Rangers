using BTG.Entity;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerTankController : IEntityController, IFixedUpdatable, IUpdatable
    {
        private PlayerService m_PlayerService;
        private PlayerModel m_Model;
        private PlayerView m_View;

        private IEntityTankBrain m_EntityBrain;
        private IEntityHealthController m_EntityHealthController;
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_Transform;

        public Transform CameraTarget => m_EntityBrain.CameraTarget;

        public int MaxHealth => m_EntityBrain.Model.MaxHealth;

        // cached values
        private Transform m_Transform;

        public PlayerTankController(PlayerService service, PlayerDataSO data)
        {
            m_PlayerService = service;
            m_Model = new PlayerModel(data);
            m_View = Object.Instantiate(data.Prefab);
            m_View.gameObject.layer = data.SelfLayer;
            m_Transform = m_View.transform;

            CreateHealthController();
        }

        ~PlayerTankController()
        {
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);

            UnsubscribeFromEntityEvents();

            m_EntityHealthController.OnHealthUpdated += OnEntityHealthUpdated;
        }

        /// <summary>
        /// Set the entity brain for the controller.
        /// </summary>
        public void SetEntityBrain(IEntityBrain entity)
        {
            m_EntityBrain = entity as IEntityTankBrain;
            if (m_EntityBrain == null)
            {
                Debug.LogError("PlayerTankController: Entity brain is not of type IEntityTankBrain");
                return;
            }

            m_EntityBrain.Model.IsPlayer = true;
            m_EntityBrain.SetParentOfView(Transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetRigidbody(Rigidbody);

            CreateCollider();
            SubscribeToEntityEvents();

            UnityMonoBehaviourCallbacks.Instance.RegisterToFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
        }

        /// <summary>
        /// Initialize the controller.
        /// </summary>
        public void Init()
        {
            m_Model.IsEnabled = true;
            m_Model.EntityMaxSpeed = m_EntityBrain.Model.MaxSpeed;
            m_Model.EntityRotateSpeed = m_EntityBrain.Model.RotateSpeed;
            m_Model.EntityAcceleration = m_EntityBrain.Model.Acceleration;

            Transform.position = Vector3.zero;
            Transform.rotation = Quaternion.identity;

            Rigidbody.centerOfMass = m_EntityBrain.Transform.position;
            Rigidbody.maxLinearVelocity = m_EntityBrain.Model.MaxSpeed;

            m_EntityHealthController.OnHealthUpdated += OnEntityHealthUpdated;
            m_EntityHealthController.Reset();
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

            m_EntityBrain?.StartPrimaryFire();
        }

        public void StopFire()
        {
            if (!m_Model.IsEnabled)
                return;

            m_EntityBrain?.StopPrimaryFire();
        }

        public void TryExecuteUltimate()
        {
            if (!m_Model.IsEnabled)
                return;

            m_EntityBrain?.TryExecuteUltimate();
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

        public void Die()
        {
            m_Model.IsEnabled = false;
            m_EntityBrain.Die();
            UnsubscribeFromEntityEvents();
            m_EntityBrain = null;
            m_EntityHealthController.OnHealthUpdated -= OnEntityHealthUpdated;

            m_PlayerService.OnPlayerDeath();

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
        }

        private void CreateHealthController()
        {
            m_EntityHealthController = m_View.gameObject.AddComponent<EntityHealthController>();
            m_EntityHealthController.SetController(this);
        }

        private void OnEntityHealthUpdated(int currentHealth, int maxHealth)
            => m_Model.PlayerData.OnPlayerHealthUpdated.RaiseEvent(currentHealth, maxHealth);

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
            => m_Model.Acceleration = m_Model.EntityAcceleration * m_Model.MoveInputValue;

        private void CreateCollider()
        {
            string name = m_View.gameObject.name;
            Component collider = m_View.gameObject.AddComponent(m_EntityBrain.DamageCollider.GetType());
            HelperMethods.CopyComponentProperties(m_EntityBrain.DamageCollider, collider);
            m_View.gameObject.name = name;
            m_EntityHealthController.SetCollider(collider as Collider);
        }

        private void OnEntityVisibilityToggled(bool value) => m_EntityHealthController.ToggleCollider(value);

        private void SubscribeToEntityEvents()
        {
            m_EntityBrain.OnEntityInitialized += m_PlayerService.OnEntityInitialized;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned += m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated += m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged += m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;
            m_EntityBrain.OnEntityVisibilityToggled += OnEntityVisibilityToggled;
        }

        private void UnsubscribeFromEntityEvents()
        {
            m_EntityBrain.OnEntityInitialized -= m_PlayerService.OnEntityInitialized;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned -= m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated -= m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged -= m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted -= m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;
            m_EntityBrain.OnEntityVisibilityToggled -= OnEntityVisibilityToggled;
        }
    }
}
