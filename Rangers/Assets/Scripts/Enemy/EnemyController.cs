using BTG.Entity;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;
using VContainer;


namespace BTG.Enemy
{
    /// <summary>
    /// A Controller for the enemy tank
    /// </summary>
    public class EnemyController : IEntityController
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
        public IDamageableView TargetView { get; private set; }

        /// <summary>
        /// Is Ultimate ready to be executed
        /// </summary>
        public bool IsUltimateReady { get; private set; }

        /// <summary>
        /// Get the layer of the opposition
        /// </summary>
        public int OppositionLayer => 1 << m_Data.OppositionLayerMask;

        /// <summary>
        /// Get the max health of the entity
        /// </summary>
        public int MaxHealth => m_EntityBrain.Model.MaxHealth;

        /// <summary>
        /// Get the Ultiamte tag of the entity
        /// </summary>
        public TagSO UltimateTag => m_EntityBrain.Model.UltimateTag;

        [Inject]
        private EnemyService m_Service;

        private EnemyPool m_Pool;
        private IEntityBrain m_EntityBrain;
        private EntityHealthController m_EntityHealthController;
        private EnemyView m_View;
        private EnemyTankStateMachine m_StateMachine;

        public class Builder
        {
            private EnemyDataSO data;
            private EnemyPool pool;
            private EnemyView view;
            private NavMeshAgent agent;
            private EnemyTankStateMachine stateMachine;

            public Builder WithEnemyData(EnemyDataSO data)
            {
                this.data = data;
                return this;
            }

            public Builder WithEnemyPool(EnemyPool pool)
            {
                this.pool = pool;
                return this;
            }

            public Builder WithEnemyView(EnemyView view)
            {
                this.view = view;
                return this;
            }

            public Builder WithNavMeshAgent(NavMeshAgent agent)
            {
                this.agent = agent;
                return this;
            }

            public Builder WithStateMachine(EnemyTankStateMachine stateMachine)
            {
                this.stateMachine = stateMachine;
                return this;
            }

            public EnemyController Build()
            {
                return new EnemyController
                {
                    m_Data = data,
                    m_Pool = pool,
                    m_View = view,
                    m_Agent = agent,
                    m_StateMachine = stateMachine,
                };
            }
        }

        private EnemyController() { }

        ~EnemyController()
        {
            UnsubscribeFromEvents();
            m_EntityBrain = null;
        }

        public void SetMaxLinearVelocity(float maxLinearVelocity) 
            => Rigidbody.maxLinearVelocity = maxLinearVelocity;

        /// <summary>
        /// Set the pose of the controller's view
        /// </summary>
        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

        /// <summary>
        /// Set the entity brain and it's properties to the controller
        /// </summary>
        public void SetEntityBrain(IEntityBrain entity)
        {
            m_EntityBrain = entity;
            if (m_EntityBrain == null)
            {
                Debug.LogError("EnemyTankController: SetEntityBrain: EntityBrain is not of type IEntityTankBrain");
                return;
            }

            m_EntityBrain.Model.IsPlayer = false;
            m_EntityBrain.SetParentOfView(m_View.transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetRigidbody(Rigidbody);
            m_EntityBrain.SetDamageable(m_EntityHealthController as IDamageableView);
            m_EntityBrain.SetOppositionLayerMask(m_Data.OppositionLayerMask);

            m_EntityBrain.PrimaryAction.OnPrimaryActionExecuted += m_StateMachine.OnPrimaryActionExecuted;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += OnUltimateExecuted;
            m_EntityBrain.UltimateAction.OnFullyCharged += OnUltimateFullyCharged;

            /// This need to be subscribed here, to ensure that when the entity is initialized, the visibility of the UI is toggled
            m_EntityBrain.OnEntityVisibilityToggled += m_View.ToggleUIVisibility;

            m_EntityBrain.Init();
        }

        /// <summary>
        /// Initialize the controller after the Entity is set
        /// it configures the IEntityHealthCOntroller with damage collider of entity
        /// It initializes properties of NavMeshAgent
        /// It initializes the state machine
        /// It subscribe to events
        /// </summary>
        public void Init()
        {
            if (m_EntityBrain == null)
            {
                Debug.LogError("First initialize the entity brain");
                return;
            }

            InitializeController();
            InitializeHealthAndDamage();
            InitializeAgent();
            InitializeStateMachine();
        }

        /// <summary>
        /// Inform the controller that the player entered or exited the range of the entity
        /// </summary>
        public void UpdatePlayerView(IDamageableView view)
        {
            TargetView = view;
            if (TargetView == null)
            {
                m_StateMachine.OnTargetNotInRange();
            }
            else
            {
                m_StateMachine.OnTargetInRange();
            }
        }

        /// <summary>
        /// Execute the primary action
        /// </summary>
        public void ExecutePrimaryAction(int stopTime) => m_EntityBrain.AutoStartStopPrimaryAction(stopTime);

        /// <summary>
        /// Execute the ultimate action
        /// </summary>
        public void ExecuteUltimateAction()
        {
            m_EntityBrain.TryExecuteUltimate();
            IsUltimateReady = false;
        }

        /// <summary>
        /// Reallign the entity brain's transform with the rigidbody's transform
        /// </summary>
        public void ReAllign()
        {
            m_EntityBrain.Transform.position = Rigidbody.position;
            m_EntityBrain.Transform.rotation = Rigidbody.rotation;
        }

        public void OnEntityDied()
        {
            m_EntityBrain.OnDead();

            m_StateMachine.OnEntityDead();
            m_EntityBrain.DeInit();
            UnsubscribeFromEvents();
            m_EntityBrain = null;
            m_View.ToggleView(false);
            m_Pool.ReturnEnemy(this);

            m_Service.OnEnemyDeath();
        }


        private void InitializeController()
        {
            IsUltimateReady = false;
            m_View.ToggleView(true);
        }

        private void InitializeHealthAndDamage()
        {
            m_EntityBrain.DamageCollider.gameObject.layer = m_Data.SelfLayer;
            m_EntityHealthController = (EntityHealthController)m_EntityBrain.DamageCollider.gameObject.GetOrAddComponent<EntityHealthController>();
            m_EntityHealthController.SetController(this);
            m_EntityHealthController.SetOwner(m_EntityBrain.Transform, false);
            m_EntityHealthController.IsEnabled = true;

            m_EntityHealthController.OnDamageTaken += OnDamageTaken;
            m_EntityHealthController.OnHealthUpdated += m_View.UpdateHealthUI;

            ///This need to be subscribed here to ensure that the m_EntityHealthController is initialized
            m_EntityBrain.OnEntityVisibilityToggled += m_EntityHealthController.SetVisible;
            // The brain is already been initialized, so the visibility of the health controller is set here
            m_EntityHealthController.SetVisible(true);      

            m_EntityHealthController.SetMaxHealth();
        }

        private void InitializeAgent()
        {
            m_Agent.speed = m_EntityBrain.Model.MaxSpeed * m_Data.MaxSpeedMultiplier;
            m_Agent.stoppingDistance = m_Data.StoppingDistance;
        }

        private void InitializeStateMachine()
        {
            m_StateMachine.CreateStates();
            m_StateMachine.Init(m_Data.InitialState);
        }

        private void OnUltimateFullyCharged() => IsUltimateReady = true;
        
        private void OnUltimateExecuted() => m_StateMachine.OnUltimateExecuted();

        private void OnDamageTaken() => m_StateMachine.OnDamageTaken();

        private void UnsubscribeFromEvents()
        {
            m_EntityBrain.PrimaryAction.OnPrimaryActionExecuted -= m_StateMachine.OnPrimaryActionExecuted;
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted -= OnUltimateExecuted;
            m_EntityBrain.UltimateAction.OnFullyCharged -= OnUltimateFullyCharged;
            
            m_EntityHealthController.OnDamageTaken -= OnDamageTaken;
            m_EntityHealthController.OnHealthUpdated -= m_View.UpdateHealthUI;

            /// The below two events are subscribed in different methods, first one in SetEntity method, second one in InitializeHealthAndDamage method
            /// So they needed to be unsubscribed in here separately.
            m_EntityBrain.OnEntityVisibilityToggled -= m_View.ToggleUIVisibility;
            m_EntityBrain.OnEntityVisibilityToggled -= m_EntityHealthController.SetVisible;
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
