using BTG.Entity;
using BTG.Utilities;
using UnityEngine;
using VContainer;


namespace BTG.Player
{
    public class PlayerTankController : IEntityController, IFixedUpdatable, IUpdatable
    {
        [Inject]
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

        public PlayerTankController(PlayerDataSO data)
        {
            m_Model = new PlayerModel(data);
            m_View = Object.Instantiate(data.Prefab);
            m_Transform = m_View.transform;
        }

        ~PlayerTankController()
        {
            UnsubscribeFromEvents();
        }

        /// <summary>
        /// Set the pose of the controller's view
        /// </summary>
        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

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
            m_EntityBrain.SetDamageable(m_EntityHealthController);
            m_EntityBrain.SetOppositionLayerMask(m_Model.PlayerData.OppositionLayerMask);

            m_EntityBrain.OnEntityInitialized += m_PlayerService.OnEntityInitialized;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned += m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated += m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged += m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            m_EntityBrain.Init();
        }

        /// <summary>
        /// Initialize the controller. after the entity is set.
        /// It configures the health and damage.
        /// It subscribes to events
        /// </summary>
        public void Init()
        {
            InitializeDatas();
            InitializeHealthAndDamage();
            SubscribeToEvents();
        }

        /// <summary>
        /// Deinitialize the controller and it's entity brain.
        /// </summary>
        public void DeInit()
        {
            // If the entity brain is null, then the entity is already deinitialized.
            if (m_EntityBrain == null) return;

            m_Model.IsAlive = false;
            m_EntityBrain.DeInit();
            UnsubscribeFromEvents();
            m_EntityBrain = null;
        }

        public void SetMoveValue(in float value)
        {
            if (!m_Model.IsAlive)
                return;

            m_Model.MoveInputValue = value;
        }

        public void SetRotateValue(in float value)
        {
            if (!m_Model.IsAlive)
                return;

            m_Model.RotateInputValue = value;
        }

        public void StartFire()
        {
            if (!m_Model.IsAlive)
                return;

            m_EntityBrain?.StartPrimaryAction();
        }

        public void StopFire()
        {
            if (!m_Model.IsAlive)
                return;

            m_EntityBrain?.StopPrimaryAction();
        }

        public void TryExecuteUltimate()
        {
            if (!m_Model.IsAlive)
                return;

            m_EntityBrain?.TryExecuteUltimate();
        }

        public void FixedUpdate()
        {
            if (!m_Model.IsAlive)
                return;

            Rotate();

            MoveWithForce();
        }

        public void Update()
        {
            if (!m_Model.IsAlive)
                return;

            CalculateInputSpeed();
        }

        public void OnEntityDied()
        {
            m_EntityBrain.OnDead();

            DeInit();
            m_PlayerService.OnPlayerDeath();
        }

        private void InitializeDatas()
        {
            m_Model.IsAlive = true;
            m_Model.EntityMaxSpeed = m_EntityBrain.Model.MaxSpeed;
            m_Model.EntityRotateSpeed = m_EntityBrain.Model.RotateSpeed;
            m_Model.EntityAcceleration = m_EntityBrain.Model.Acceleration;

            Rigidbody.centerOfMass = Vector3.zero;
            Rigidbody.maxLinearVelocity = m_EntityBrain.Model.MaxSpeed;
        }

        private void InitializeHealthAndDamage()
        {
            m_EntityBrain.DamageCollider.gameObject.layer = m_Model.PlayerData.SelfLayer;
            m_EntityHealthController = m_EntityBrain.DamageCollider.gameObject.GetOrAddComponent<EntityHealthController>() as IEntityHealthController;
            m_EntityHealthController.SetController(this);
            m_EntityHealthController.SetOwner(m_EntityBrain.Transform, true);
            m_EntityHealthController.IsEnabled = true;

            /// This one needs to be set after the health controller is initialized
            m_EntityBrain.OnEntityVisibilityToggled += m_EntityHealthController.SetVisible;
            /// The m_EntityBrain has already been initialized, so we need to set the visibility of the health controller
            m_EntityHealthController.SetVisible(true);

            m_EntityHealthController.OnHealthUpdated += OnEntityHealthUpdated;

            m_EntityHealthController.SetMaxHealth();
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

        private void SubscribeToEvents()
        {
            UnityMonoBehaviourCallbacks.Instance.RegisterToFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.RegisterToUpdate(this);
        }

        private void UnsubscribeFromEvents()
        {
            m_EntityBrain.OnEntityInitialized -= m_PlayerService.OnEntityInitialized;
            m_EntityBrain.OnEntityVisibilityToggled += m_EntityHealthController.SetVisible;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned -= m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated -= m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged -= m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted -= m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            m_EntityHealthController.OnHealthUpdated -= OnEntityHealthUpdated;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);
        }
    }
}
