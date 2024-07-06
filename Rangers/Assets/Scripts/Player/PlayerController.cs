using BTG.Entity;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerController : IEntityController, IFixedUpdatable, IUpdatable
    {
        private PlayerModel m_Model;
        private PlayerView m_View;
        private PlayerInputs m_PlayerInputs;
        private PlayerService m_PlayerService;
        private IEntityBrain m_EntityBrain;
        private EntityHealthController m_EntityHealthController;
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Vector3 Velocity => Rigidbody.velocity;
        public Transform Transform => m_Transform;

        public Transform CameraTarget => m_EntityBrain.CameraTarget;

        public int MaxHealth => m_EntityBrain.Model.MaxHealth;

        // cached values
        private Transform m_Transform;

        private PlayerController() { }

        public class Builder
        {
            private PlayerModel model;
            private PlayerView view;
            private Transform transForm;
            private PlayerService playerService;
            private PlayerInputs playerInput;

            public Builder WithModel(PlayerModel model)
            {
                this.model = model;
                return this;
            }

            public Builder CreateModel(PlayerDataSO data)
            {
                model = new PlayerModel(data);
                return this;
            }

            public Builder WithView(PlayerView view)
            {
                this.view = view;
                transForm = view.transform;
                return this;
            }

            public Builder CreateView(PlayerView prefab)
            {
                view = Object.Instantiate(prefab);
                transForm = view.transform;
                return this;
            }

            public Builder WithPlayerService(PlayerService playerService)
            {
                this.playerService = playerService;
                return this;
            }

            public Builder CreatePlayerInput()
            {
                playerInput = new PlayerInputs();
                playerInput.Initialize();
                return this;
            }

            public PlayerController Build()
            {
                var controller = new PlayerController();
                controller.m_Model = model;
                controller.m_View = view;
                controller.m_Transform = transForm;
                controller.m_PlayerService = playerService;
                controller.m_PlayerInputs = playerInput;
                
                return controller;
            }
        }

        ~PlayerController()
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
            m_EntityBrain = entity;
            if (m_EntityBrain == null)
            {
                Debug.LogError("PlayerTankController: Entity brain is not of type IEntityTankBrain");
                return;
            }

            m_EntityBrain.Model.IsPlayer = true;
            m_EntityBrain.SetController(this);
            m_EntityBrain.SetParentOfView(Transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetDamageable(m_EntityHealthController);
            m_EntityBrain.SetOppositionLayerMask(m_Model.PlayerData.OppositionLayerMask);

            m_EntityBrain.OnEntityInitialized += m_PlayerService.OnEntityInitialized;

            m_EntityBrain.PrimaryAction.OnActionAssigned += m_Model.PlayerData.OnPrimaryActionAssigned.RaiseEvent;
            m_EntityBrain.PrimaryAction.OnActionStarted += m_Model.PlayerData.OnPrimaryActionStarted.RaiseEvent;
            m_EntityBrain.PrimaryAction.OnActionChargeUpdated += m_Model.PlayerData.OnPrimaryActionChargeUpdated.RaiseEvent;
            m_EntityBrain.PrimaryAction.OnActionExecuted += m_Model.PlayerData.OnPrimaryActionExecuted.RaiseEvent;

            m_EntityBrain.UltimateAction.OnActionAssigned += m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated += m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged += m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnActionExecuted += m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

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
            m_Model.IsAlive = false;
        }

        public void DeInitEntity()
        {
            if (m_EntityBrain == null) return;
            UnsubscribeFromEvents();
            m_EntityBrain.DeInit();
            m_EntityBrain = null;
        }

        public void SetMoveValue(float value)
        {
            if (!m_Model.IsAlive)
                return;

            m_Model.MoveInputValue = value;
        }

        public void SetRotateValue(float value)
        {
            if (!m_Model.IsAlive)
                return;

            m_Model.RotateInputValue = value;
        }

        public void StartPrimaryAction()
        {
            if (!m_Model.IsAlive)
                return;

            m_EntityBrain?.StartPrimaryAction();
        }

        public void StopPrimaryAction()
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
            m_EntityBrain.ExecuteRagdollEffectEvent();
            DeInitEntity();
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
            m_EntityHealthController = (EntityHealthController)m_EntityBrain.DamageCollider.gameObject.GetOrAddComponent<EntityHealthController>();
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

            m_PlayerInputs.OnMoveInput += SetMoveValue;
            m_PlayerInputs.OnRotateInput += SetRotateValue;
            m_PlayerInputs.OnPrimaryActionInputStarted += StartPrimaryAction;
            m_PlayerInputs.OnPrimaryActionInputCanceled += StopPrimaryAction;
            m_PlayerInputs.OnUltimateInputPerformed += TryExecuteUltimate;
        }

        private void UnsubscribeFromEvents()
        {
            m_EntityBrain.OnEntityInitialized -= m_PlayerService.OnEntityInitialized;
            m_EntityBrain.OnEntityVisibilityToggled -= m_EntityHealthController.SetVisible;

            m_EntityBrain.PrimaryAction.OnActionAssigned -= m_Model.PlayerData.OnPrimaryActionAssigned.RaiseEvent;
            m_EntityBrain.PrimaryAction.OnActionStarted -= m_Model.PlayerData.OnPrimaryActionStarted.RaiseEvent;
            m_EntityBrain.PrimaryAction.OnActionChargeUpdated -= m_Model.PlayerData.OnPrimaryActionChargeUpdated.RaiseEvent;
            m_EntityBrain.PrimaryAction.OnActionExecuted -= m_Model.PlayerData.OnPrimaryActionExecuted.RaiseEvent;

            m_EntityBrain.UltimateAction.OnActionAssigned -= m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated -= m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged -= m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnActionExecuted -= m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            m_EntityHealthController.OnHealthUpdated -= OnEntityHealthUpdated;

            UnityMonoBehaviourCallbacks.Instance.UnregisterFromFixedUpdate(this);
            UnityMonoBehaviourCallbacks.Instance.UnregisterFromUpdate(this);

            m_PlayerInputs.OnMoveInput -= SetMoveValue;
            m_PlayerInputs.OnRotateInput -= SetRotateValue;
            m_PlayerInputs.OnPrimaryActionInputStarted -= StartPrimaryAction;
            m_PlayerInputs.OnPrimaryActionInputCanceled -= StopPrimaryAction;
            m_PlayerInputs.OnUltimateInputPerformed -= TryExecuteUltimate;
        }
    }
}
