using BTG.Entity;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities;
using Unity.Netcode;
using UnityEngine;


namespace BTG.Gameplay.GameplayObjects
{
    /// <summary>
    /// This is a fused script of SinglePlayer's PlayerTankController and PlayerView
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NetworkAvatarGuidState))]
    public class NetworkPlayerView : NetworkBehaviour, IEntityController
    {
        private Rigidbody m_Rigidbody;
        private NetworkAvatarGuidState m_NetworkAvatarGuidState;
        private Pose m_SpawnPose;
        private PlayerModel m_Model;
        private IPlayerService m_PlayerService;
        private TankView m_GraphicsView;
        private IEntityTankBrain m_EntityBrain;
        private IEntityHealthController m_EntityHealthController;
        private PlayerInputs m_PlayerInputs;

        public TagSO Tag => m_NetworkAvatarGuidState.RegisteredEntityData.Tag;

        public int MaxHealth => m_EntityBrain.Model.MaxHealth;

        public Transform CameraTarget
        {
            get
            {
                if (!IsOwner)
                {
                    Debug.Log("CameraTarget is not available for non-owners");
                }

                if (IsServer)
                {
                    return m_EntityBrain.CameraTarget;
                }
                else
                {
                    return m_GraphicsView.CameraTarget;
                }
            }
        }

        private NetworkVariable<float> mn_MoveValue = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
        private NetworkVariable<float> mn_RotateValue = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
        private NetworkVariable<bool> mn_IsAlive = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_NetworkAvatarGuidState = GetComponent<NetworkAvatarGuidState>();
            m_EntityHealthController = GetComponent<EntityHealthController>();
        }

        private void FixedUpdate()
        {
            if (!IsServer)
                return;

            if (!mn_IsAlive.Value)
                return;

            Rotate();

            MoveWithForce();
        }

        /*private void Update()
        {
            if (!IsServer)
                return;

            if (!m_Model.IsAlive)
                return;

            CalculateInputSpeed();
        }*/

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (IsOwner)
                UnsubscribeFromInputEvents();
            
            if (IsServer)
                UnsubscribeFromEntityEvents();

            if (m_GraphicsView != null)
                Destroy(m_GraphicsView);
        }

        public void SetPlayerModel(PlayerModel model) => m_Model = model;
        public void SetPlayerService(IPlayerService service) => m_PlayerService = service;
        public void SetPlayerInputs(PlayerInputs inputs)
        {
            m_PlayerInputs = inputs;
            SubscribeToInputEvents();
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
            m_EntityBrain.SetParentOfView(transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetRigidbody(m_Rigidbody);
            m_EntityBrain.SetDamageable(m_EntityHealthController);
            m_EntityBrain.SetOppositionLayerMask(m_Model.PlayerData.OppositionLayerMask);

            m_EntityBrain.OnEntityInitialized += m_PlayerService.OnEntityInitialized;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned += m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated += m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged += m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            m_EntityBrain.Init();

            CacheEntityDatas();
            ConfigureEntityWithHealthController();
        }

        public void Init()
        {
            mn_IsAlive.Value = true;
        }

        public void SpawnGraphics()
        {
            m_GraphicsView = Instantiate(m_NetworkAvatarGuidState.RegisteredEntityData.Graphics, transform).GetComponent<TankView>();
            m_GraphicsView.transform.localPosition = Vector3.zero;
            m_GraphicsView.transform.localRotation = Quaternion.identity;
        }

        public void OnEntityDied()
        {
            m_EntityBrain.OnDead();

            DeInit();
            m_PlayerService.OnPlayerDeath();
        }

        /// <summary>
        /// Deinitialize the controller and it's entity brain.
        /// </summary>
        public void DeInit()
        {
            // If the entity brain is null, then the entity is already deinitialized.
            if (m_EntityBrain == null) return;

            mn_IsAlive.Value = false;
            m_EntityBrain.DeInit();
            UnsubscribeFromEntityEvents();
            m_EntityBrain = null;
        }

        private void CacheEntityDatas()
        {
            m_Model.EntityMaxSpeed = m_EntityBrain.Model.MaxSpeed;
            m_Model.EntityRotateSpeed = m_EntityBrain.Model.RotateSpeed;
            m_Model.EntityAcceleration = m_EntityBrain.Model.Acceleration;

            m_Rigidbody.centerOfMass = Vector3.zero;
            m_Rigidbody.maxLinearVelocity = m_EntityBrain.Model.MaxSpeed;
        }

        private void ConfigureEntityWithHealthController()
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
            m_Rigidbody.AddForce(transform.forward * m_Model.EntityAcceleration * mn_MoveValue.Value, ForceMode.Acceleration);
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_Model.EntityMaxSpeed);
        }

        private void Rotate()
        {
            m_Model.RotateAngle = m_Model.EntityRotateSpeed * mn_RotateValue.Value * Time.fixedDeltaTime *
                (mn_MoveValue.Value > 0 ? 1 :
                    (mn_MoveValue.Value < 0 ? -1 : 0)
                    );

            m_Model.DeltaRotation = Quaternion.Euler(0, m_Model.RotateAngle, 0);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * m_Model.DeltaRotation);
        }

        /*private void CalculateInputSpeed()
            => m_Model.Acceleration = m_Model.EntityAcceleration * m_Model.MoveInputValue;*/

        private void SetMoveValue(float value)
        {
            if (!mn_IsAlive.Value)
                return;

            // m_Model.MoveInputValue = value;
            mn_MoveValue.Value = value;
        }

        private void SetRotateValue(float value)
        {
            if (!mn_IsAlive.Value)
                return;

            // m_Model.RotateInputValue = value;
            mn_RotateValue.Value = value;
        }

        private void StartPrimaryAction()
        {
            if (!mn_IsAlive.Value)
                return;

            m_EntityBrain?.StartPrimaryAction();
        }

        private void StopPrimaryAction()
        {
            if (!mn_IsAlive.Value)
                return;

            m_EntityBrain?.StopPrimaryAction();
        }

        private void TryExecuteUltimate()
        {
            if (!mn_IsAlive.Value)
                return;

            m_EntityBrain?.TryExecuteUltimate();
        }

        private void SubscribeToInputEvents()
        {
            m_PlayerInputs.OnMoveInput += SetMoveValue;
            m_PlayerInputs.OnRotateInput += SetRotateValue;
            m_PlayerInputs.OnPrimaryActionInputStarted += StartPrimaryAction;
            m_PlayerInputs.OnPrimaryActionInputCanceled += StopPrimaryAction;
            m_PlayerInputs.OnUltimateInputPerformed += TryExecuteUltimate;
        }

        private void UnsubscribeFromInputEvents()
        {
            m_PlayerInputs.OnMoveInput -= SetMoveValue;
            m_PlayerInputs.OnRotateInput -= SetRotateValue;
            m_PlayerInputs.OnPrimaryActionInputStarted -= StartPrimaryAction;
            m_PlayerInputs.OnPrimaryActionInputCanceled -= StopPrimaryAction;
            m_PlayerInputs.OnUltimateInputPerformed -= TryExecuteUltimate;
        }

        private void UnsubscribeFromEntityEvents()
        {
            m_EntityBrain.OnEntityInitialized -= m_PlayerService.OnEntityInitialized;
            m_EntityBrain.OnEntityVisibilityToggled += m_EntityHealthController.SetVisible;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned -= m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated -= m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged -= m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted -= m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            m_EntityHealthController.OnHealthUpdated -= OnEntityHealthUpdated;
        }
    }

}