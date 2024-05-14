using BTG.Entity;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;
using UnityEngine.AI;


namespace BTG.Enemy
{
    /// <summary>
    /// A Controller for the enemy tank
    /// </summary>
    public class EnemyTankController : IEntityController
    {
        private EnemyDataSO m_Data;
        /// <summary>
        /// Get the data of the enemy
        /// </summary>
        public EnemyDataSO Data => m_Data;

        private NavMeshAgent m_Agent;
        /// <summary>
        /// Get the agent of the enemy
        /// </summary>
        public NavMeshAgent Agent => m_Agent;

        /// <summary>
        /// Get the rigidbody of the enemy
        /// </summary>
        public Rigidbody Rigidbody => m_View.Rigidbody;

        /// <summary>
        /// Get the transform of the enemy
        /// </summary>
        public Transform Transform => m_View.transform;

        /// <summary>
        /// Get the target view that is in range and detected
        /// </summary>
        public IPlayerView TargetView { get; private set; }

        /// <summary>
        /// Is Ultimate ready to be executed
        /// </summary>
        public bool IsUltimateReady { get; private set; }

        /// <summary>
        /// Get the layer of the opposition
        /// </summary>
        public int OppositionLayer => 1 << m_Data.OppositionLayerMask;

        /// <summary>
        /// Get the max health of the enemy
        /// </summary>
        public int MaxHealth => m_EntityBrain.Model.MaxHealth;

        /// <summary>
        /// Get the Ultiamte tag of the entity
        /// </summary>
        public TagSO UltimateTag => m_EntityBrain.Model.UltimateTag;

        private EnemyPool m_Pool;
        private EnemyService m_Service;
        private IEntityTankBrain m_EntityBrain;
        private IEntityHealthController m_EntityHealthController;
        private EnemyView m_View;
        private EnemyTankStateMachine m_StateMachine;


        public EnemyTankController(EnemyDataSO data, EnemyPool pool)
        {
            m_Pool = pool;
            m_Data = data;
            m_View = Object.Instantiate(m_Data.EnemyPrefab, pool.EnemyContainer);
            m_View.SetController(this);

            m_Agent = m_View.GetComponent<NavMeshAgent>();

            m_StateMachine = new (this);
            DIManager.Instance.Inject(m_StateMachine);

            Rigidbody.maxLinearVelocity = m_Data.MaxSpeedMultiplier * m_Data.MaxSpeedMultiplier;
        }

        

        ~EnemyTankController()
        {
            UnsubscribeFromEntityEvents();
            m_EntityBrain = null;
        }

        /// <summary>
        /// Initialize the controller
        /// It sets the speed and stopping distance of the agent
        /// It initializes the state machine
        /// </summary>
        public void Init()
        {
            m_EntityHealthController.Reset();

            m_Agent.speed = m_EntityBrain.Model.MaxSpeed * m_Data.MaxSpeedMultiplier;
            m_Agent.stoppingDistance = m_Data.StoppingDistance;

#if UNITY_EDITOR
            if (m_Data.InitializeState)
            {
                m_StateMachine.Init(m_Data.InitialState);
            }
#endif
        }

        /// <summary>
        /// Set the pose of the controller's view
        /// </summary>
        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

        /// <summary>
        /// Set the player view that has been detected
        /// </summary>
        public void SetPlayerView(IPlayerView view) => TargetView = view;

        /// <summary>
        /// Set the entity brain and it's properties to the controller
        /// </summary>
        public void SetEntityBrain(IEntityBrain entity)
        {
            m_EntityBrain = entity as IEntityTankBrain;
            
            if (m_EntityBrain == null)
            {
                Debug.LogError("EnemyTankController: SetEntityBrain: EntityBrain is not of type IEntityTankBrain");
                return;
            }

            m_EntityBrain.Model.IsPlayer = false;
            m_EntityBrain.SetParentOfView(m_View.transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetRigidbody(Rigidbody);
            m_EntityBrain.SetDamageable(m_EntityHealthController as IDamageable);
            m_EntityBrain.SetOppositionLayerMask(m_Data.OppositionLayerMask);

            IntializeDamageCollider();
            m_StateMachine.CreateStates();

            SubscribeToEvents();
        }

        /// <summary>
        /// Set the enemy service
        /// </summary>
        public void SetService(EnemyService service) => m_Service = service;

        /// <summary>
        /// Execute the primary action
        /// </summary>
        public void ExecutePrimaryAction() => m_EntityBrain.StartPrimaryFire();

        /// <summary>
        /// Execute the ultimate action
        /// </summary>
        public void ExecuteUltimateAction() => m_EntityBrain.TryExecuteUltimate();

        /// <summary>
        /// Reallign the entity brain's transform with the rigidbody's transform
        /// </summary>
        public void ReAllign()
        {
            m_EntityBrain.Transform.position = Rigidbody.position;
            m_EntityBrain.Transform.rotation = Rigidbody.rotation;
        }

        public void EntityDied()
        {
            m_StateMachine.OnEntityDead();
            m_EntityBrain.DeInit();
            UnsubscribeFromEntityEvents();
            m_EntityBrain = null;
            m_Pool.ReturnEnemy(this);

            m_Service.OnEnemyDeath();
        }

        private void IntializeDamageCollider()
        {
            /*string name = m_View.gameObject.name;
            Component collider = m_View.gameObject.AddComponent(m_EntityBrain.DamageCollider.GetType());
            HelperMethods.CopyComponentProperties(m_EntityBrain.DamageCollider, collider);
            m_View.gameObject.name = name;
            m_EntityHealthController.SetCollider(collider as Collider);*/

            m_EntityBrain.DamageCollider.gameObject.layer = m_Data.SelfLayer;
            m_EntityHealthController = m_EntityBrain.DamageCollider.gameObject.GetOrAddComponent<EntityHealthController>() as IEntityHealthController;
            m_EntityHealthController.SetController(this);
        }

        private void OnUltimateFullyCharged() => IsUltimateReady = true;
        
        private void OnUltimateExecuted()
        {
            IsUltimateReady = false;
            m_StateMachine.OnUltimateExecuted();
        }

        private void OnDamageTaken() => m_StateMachine.OnDamageTaken();

        private void OnEntityVisibilityToggled(bool value)
        {
            m_View.ToggleVisibility(value);
        }

        private void SubscribeToEvents()
        {
            m_EntityBrain.PrimaryAction.OnPrimaryActionExecuted += m_StateMachine.OnPrimaryActionExecuted;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += OnUltimateExecuted;
            m_EntityBrain.UltimateAction.OnFullyCharged += OnUltimateFullyCharged;
            m_EntityBrain.OnEntityVisibilityToggled += OnEntityVisibilityToggled;
            m_EntityHealthController.OnDamageTaken += OnDamageTaken;
            m_EntityHealthController.OnHealthUpdated += m_View.UpdateHealthUI;
        }

        private void UnsubscribeFromEntityEvents()
        {
            m_EntityBrain.PrimaryAction.OnPrimaryActionExecuted -= m_StateMachine.OnPrimaryActionExecuted;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted -= OnUltimateExecuted;
            m_EntityBrain.UltimateAction.OnFullyCharged -= OnUltimateFullyCharged;
            m_EntityBrain.OnEntityVisibilityToggled -= OnEntityVisibilityToggled;
            m_EntityHealthController.OnDamageTaken -= OnDamageTaken;
            m_EntityHealthController.OnHealthUpdated -= m_View.UpdateHealthUI;
        }

#if UNITY_EDITOR

        /// <summary>
        /// This method is used to log the description of the view in it's inspector
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="append">if true, the message will be added as new line, or will replace the old message</param>
        public void LogDescription(string message) => m_View.LogDescription(message);

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Transform.position, m_Agent.destination);
        }
#endif
    }
}
