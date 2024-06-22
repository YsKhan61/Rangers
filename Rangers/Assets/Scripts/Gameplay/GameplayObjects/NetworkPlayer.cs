using BTG.Actions.UltimateAction;
using BTG.Entity;
using BTG.Player;
using BTG.Tank;
using BTG.Utilities;
using Unity.Netcode;
using UnityEngine;
using VContainer;


namespace BTG.Gameplay.GameplayObjects
{
    /// <summary>
    /// This is a fused script of SinglePlayer's PlayerTankController and PlayerView
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class NetworkPlayer : NetworkBehaviour, IEntityController
    {
        [Inject]
        private EntityFactoryContainerSO m_EntityFactoryContainer;

        [Inject]
        private UltimateActionDataContainerSO m_UltimateActionDataContainer;

        private Rigidbody m_Rigidbody;
        private Pose m_SpawnPose;
        private PlayerModel m_Model;
        private NetworkPlayerService m_PlayerService;
        // private TankView m_GraphicsView;
        private IEntityTankBrain m_EntityBrain;
        private EntityHealthController m_EntityHealthController;
        private PlayerInputs m_PlayerInputs;
        private PersistentPlayer m_PersistentPlayer;
        private NetworkEntityGuidState m_NetworkEntityGuidState;
        public EntityDataSO RegisteredEntityData => m_NetworkEntityGuidState.RegisteredEntityData; // This changes at runtime

        public int MaxHealth => m_EntityBrain.Model.MaxHealth;

        public Transform CameraTarget
        {
            get
            {
                if (!IsOwner)
                {
                    Debug.Log("CameraTarget is not available for non-owners");
                }

                return m_EntityBrain.CameraTarget;

                /*if (IsServer)
                {
                    return m_EntityBrain.CameraTarget;
                }
                else
                {
                    return m_GraphicsView.CameraTarget;
                }*/
            }
        }

        private NetworkVariable<float> mn_MoveValue = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
        private NetworkVariable<float> mn_RotateValue = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
        private NetworkVariable<bool> mn_IsAlive = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner, readPerm: NetworkVariableReadPermission.Everyone);
        private NetworkVariable<int> mn_Health = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server, readPerm: NetworkVariableReadPermission.Everyone);

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_EntityHealthController = GetComponent<EntityHealthController>();
        }

        public override void OnNetworkSpawn()
        {
            m_PersistentPlayer = PersistentPlayerClientCache.GetPlayer(OwnerClientId);
            if (m_PersistentPlayer == null)
            {
                Debug.LogError("Persistent player must need to be spawned before NetworkPlayer!");
                return;
            }
            m_NetworkEntityGuidState = m_PersistentPlayer.NetworkEntityGuidState;
            m_NetworkEntityGuidState.OnEntityDataRegistered += ConfigureEntity;
            if (IsOwner)
            {
                mn_Health.OnValueChanged += OnPlayerHealthUpdateInNetwork;
            }
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

        public override void OnNetworkDespawn()
        {
            base.OnDestroy();

            m_NetworkEntityGuidState.OnEntityDataRegistered -= ConfigureEntity;

            if (IsOwner)
            {
                UnsubscribeFromInputEvents();
                mn_Health.OnValueChanged += OnPlayerHealthUpdateInNetwork;
            }
            
            if (IsServer)
            {
                DeInit();
            }
            else
            {
                DeInitNonServerEntity();
            }
        }

        public void SetPlayerModel(PlayerModel model) => m_Model = model;
        public void SetPlayerService(NetworkPlayerService service) => m_PlayerService = service;
        public void SetPlayerInputs(PlayerInputs inputs)
        {
            m_PlayerInputs = inputs;
            SubscribeToInputEvents();
        }

        public void ConfigureEntity()
        {
            Debug.Log($"OwnerClientId {OwnerClientId} Configuring entity for player.");

            if (IsServer)
            {
                DeInitEntity();
                InitEntity();
            }
            else
            {
                DeInitNonServerEntity();
                InitNonServerEntity();
            }


            // Set specific events for owners
            if (IsOwner)
            {
                // Camera target can only be set after the entity is initialized as the entity view contains the camera target
                m_PlayerService.PVCamera.SetFollowTarget(CameraTarget);
            }
        }

        /// <summary>
        /// Set the entity
        /// </summary>
        private void InitEntity()
        {
            m_EntityBrain = m_EntityFactoryContainer.GetFactory(RegisteredEntityData.Tag).GetItem() as IEntityTankBrain;
            if (m_EntityBrain == null)
            {
                Debug.LogError("PlayerTankController: Entity brain is not of type IEntityTankBrain");
                return;
            }

            if (IsOwner)
            {
                m_EntityBrain.Model.IsPlayer = true;
            }
                
            m_EntityBrain.SetParentOfView(transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetRigidbody(m_Rigidbody);
            m_EntityBrain.SetDamageable(m_EntityHealthController);
            m_EntityBrain.SetOppositionLayerMask(m_Model.PlayerData.OppositionLayerMask);

            m_EntityBrain.OnEntityInitialized += InformOnEntityInitialized; // m_PlayerService.OnEntityInitialized;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned += InformUltimateAssigned; // m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated += InformUltimateChargeUpdated; // m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged += InformUltimateFullyCharged; // m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += InformUltimateActionExecuted; // m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            CacheEntityDatas();
            ConfigureEntityWithHealthController();

            m_EntityBrain.Init();
        }

        public void Init()
        {
            mn_IsAlive.Value = true;    // for now this gets the input from the owner.
        }

        public void InitNonServerEntity()
        {
            // Later use factory to spawn the graphics
            /*m_GraphicsView = Instantiate(RegisteredEntityData.Graphics, transform).GetComponent<TankView>();
            m_GraphicsView.transform.localPosition = Vector3.zero;
            m_GraphicsView.transform.localRotation = Quaternion.identity;*/

            EntityFactorySO factory = m_EntityFactoryContainer.GetFactory(RegisteredEntityData.Tag) as EntityFactorySO;
            if (factory == null)
            {
                Debug.LogError("Entity factory is not of type EntityFactorySO");
                return;
            }
            m_EntityBrain = factory.GetNonServerItem() as IEntityTankBrain;

            if (IsOwner)
            {
               m_EntityBrain.Model.IsPlayer = true;
            }

            m_EntityBrain.SetParentOfView(transform, Vector3.zero, Quaternion.identity);
        }

        public void DeInitNonServerEntity()
        {
            /*if (m_GraphicsView == null)
                return;
            Destroy(m_GraphicsView.gameObject);*/

            if (m_EntityBrain == null)
                return;

            m_EntityBrain.DeInitNonServer();
            m_EntityBrain = null;
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
            mn_IsAlive.Value = false;
            DeInitEntity();
        }

        public void DeInitEntity()
        {
            // If the entity brain is null, then the entity is already deinitialized.
            if (m_EntityBrain == null) return;
            m_EntityBrain.DeInit();
            UnsubscribeFromEntityEvents();
            m_EntityBrain = null;
        }

        private void InformOnEntityInitialized(Sprite icon)
        {
            InformOnEntityInitialized_ClientRpc(RegisteredEntityData.Guid.ToNetworkGuid());
        }

        private void InformUltimateAssigned(TagSO tag)
        {
            InformUltimateAssigned_ClientRpc(tag.Guid.ToNetworkGuid());
        }

        private void InformUltimateChargeUpdated(int chargeAmount)
        {
            InformUltimateChargeUpdated_ClientRpc(chargeAmount);
        }

        private void InformUltimateFullyCharged()
        {
            InformUltimateFullyCharged_ClientRpc();
        }

        private void InformUltimateActionExecuted()
        {
            InformUltimateActionExecuted_ClientRpc();
        }

        [ClientRpc]
        private void InformOnEntityInitialized_ClientRpc(NetworkGuid ng)
        {
            if (IsOwner)
            {
                m_NetworkEntityGuidState.EntityDataContainer.TryGetData(ng.ToGuid(), out EntityDataSO entityData);
                m_PlayerService.PlayerStats.PlayerIcon.Value = entityData.Icon;
            }
        }

        [ClientRpc]
        private void InformUltimateAssigned_ClientRpc(NetworkGuid ngTag)
        {
            if (IsOwner)
            {
                TagSO tag = m_UltimateActionDataContainer.GetUltimateActionTagByGuid(ngTag.ToGuid());
                m_Model.PlayerData.OnUltimateAssigned.RaiseEvent(tag);
            }
        }

        [ClientRpc]
        private void InformUltimateChargeUpdated_ClientRpc(int chargeAmount)
        {
            if (IsOwner)
            {
                m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent(chargeAmount);
            }
        }

        [ClientRpc]
        private void InformUltimateFullyCharged_ClientRpc()
        {
            if (IsOwner)
            {
                m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent();
            }
        }

        [ClientRpc]
        private void InformUltimateActionExecuted_ClientRpc()
        {
            if (IsOwner)
            {
                m_Model.PlayerData.OnUltimateExecuted.RaiseEvent();
            }
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

        /// <summary>
        /// Later try to remove the max health from here and get it from the entity data
        /// </summary>
        private void OnEntityHealthUpdated(int currentHealth, int maxHealth)
        {
            if (IsServer)
                mn_Health.Value = currentHealth;
        }

        private void OnPlayerHealthUpdateInNetwork(int prevHealth, int newHealth)
            => m_Model.PlayerData.OnPlayerHealthUpdated.RaiseEvent(newHealth, RegisteredEntityData.MaxHealth);

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
            if (!IsOwner || !mn_IsAlive.Value)
                return;

            TryExecuteUltimate_ServerRpc();
        }

        [ServerRpc]
        private void TryExecuteUltimate_ServerRpc()
        {
            bool success = m_EntityBrain.TryExecuteUltimate();
            if (success)
            {
                TryExecuteUltimate_ClientRpc();
            }
        }

        [ClientRpc]
        private void TryExecuteUltimate_ClientRpc()
        {
            // NOTE - m_EntityBrain is not set on the client side(non server), it's server only
            // hence we need to find how to create EntityBrain for all clients but without views
            if (IsServer) return;   // Server already spawned it's graphics
            m_EntityBrain?.SpawnUltimateGraphics();
        }

        private void SubscribeToInputEvents()
        {
            if (m_PlayerInputs == null)
            {
                Debug.LogError("Player inputs is null!");
                return;
            }
            m_PlayerInputs.OnMoveInput += SetMoveValue;
            m_PlayerInputs.OnRotateInput += SetRotateValue;
            m_PlayerInputs.OnPrimaryActionInputStarted += StartPrimaryAction;
            m_PlayerInputs.OnPrimaryActionInputCanceled += StopPrimaryAction;
            m_PlayerInputs.OnUltimateInputPerformed += TryExecuteUltimate;
        }

        private void UnsubscribeFromInputEvents()
        {
            if (m_PlayerInputs == null)
            {
                Debug.Log("Player inputs is null!");
                return;
            }
            m_PlayerInputs.OnMoveInput -= SetMoveValue;
            m_PlayerInputs.OnRotateInput -= SetRotateValue;
            m_PlayerInputs.OnPrimaryActionInputStarted -= StartPrimaryAction;
            m_PlayerInputs.OnPrimaryActionInputCanceled -= StopPrimaryAction;
            m_PlayerInputs.OnUltimateInputPerformed -= TryExecuteUltimate;
        }

        private void UnsubscribeFromEntityEvents()
        {
            if (m_EntityBrain == null)
            {
                Debug.Log("Entity Brain is null!");
                return;
            }
            
            m_EntityBrain.OnEntityInitialized -= InformOnEntityInitialized; // m_PlayerService.OnEntityInitialized;
            m_EntityBrain.OnEntityVisibilityToggled += m_EntityHealthController.SetVisible;
            m_EntityBrain.UltimateAction.OnUltimateActionAssigned -= InformUltimateAssigned; // m_Model.PlayerData.OnUltimateAssigned.RaiseEvent;
            m_EntityBrain.UltimateAction.OnChargeUpdated -= InformUltimateChargeUpdated; // m_Model.PlayerData.OnUltimateChargeUpdated.RaiseEvent;
            m_EntityBrain.UltimateAction.OnFullyCharged -= InformUltimateFullyCharged; // m_Model.PlayerData.OnUltimateFullyCharged.RaiseEvent;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted -= InformUltimateActionExecuted; // m_Model.PlayerData.OnUltimateExecuted.RaiseEvent;

            if (m_EntityHealthController == null)
            {
                Debug.Log("Entity Health Controller is null!");
                return;
            }
            m_EntityHealthController.OnHealthUpdated -= OnEntityHealthUpdated;
        }
    }

}