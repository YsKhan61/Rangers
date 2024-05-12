using BTG.Entity;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;


namespace BTG.Enemy
{
    public class EnemyTankController
    {
        private EnemyDataSO m_Data;
        public EnemyDataSO Data => m_Data;

        private EnemyPool m_Pool;
        private EnemyService m_Service;
        private IEntityTankBrain m_EntityBrain;
        private IEntityHealthController m_EntityHealthController;
        private EnemyView m_View;
        private NavMeshAgent m_Agent;
        public NavMeshAgent Agent => m_Agent;

        private EnemyTankStateMachine m_StateMachine;
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_View.transform;

        public IPlayerView TargetView { get; private set; }
        public bool IsTargetInRange  => TargetView != null;
        public bool IsUltimateReady { get; private set; }


        public EnemyTankController(EnemyDataSO data, EnemyPool pool)
        {
            m_Pool = pool;
            m_Data = data;
            m_View = Object.Instantiate(m_Data.EnemyPrefab, pool.EnemyContainer);
            m_View.SetController(this);
            m_Agent = m_View.GetComponent<NavMeshAgent>();
            m_StateMachine = new (this);

            Rigidbody.maxLinearVelocity = m_Data.MaxSpeedMultiplier * m_Data.MaxSpeedMultiplier;
        }

        ~EnemyTankController()
        {
            m_EntityBrain.OnAfterDeath -= OnTankDeath;
            m_EntityBrain.OnEntityVisibilityToggled -= m_View.ToggleVisibility;
            m_EntityBrain.UltimateAction.OnFullyCharged -= OnUltimateFullyCharged;
            m_EntityHealthController.OnHealthUpdated -= m_View.UpdateHealthUI;
            m_EntityBrain = null;
        }

        /// <summary>
        /// Initialize the controller
        /// It sets the speed and stopping distance of the agent
        /// It initializes the state machine
        /// </summary>
        public void Init()
        {
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
        /// <param name="entity"></param>
        public void SetEntityBrain(IEntityBrain entity)
        {
            m_EntityBrain = entity as IEntityTankBrain;
            m_EntityHealthController = m_EntityBrain.HealthController;
            if (m_EntityBrain == null)
            {
                Debug.LogError("EnemyTankController: SetEntityBrain: EntityBrain is not of type ITankBrain");
                return;
            }

            m_EntityBrain.Model.IsPlayer = false;
            m_EntityBrain.SetLayers(m_Data.SelfLayer, m_Data.OppositionLayer);
            m_EntityBrain.SetParentOfView(m_View.transform, Vector3.zero, Quaternion.identity);
            m_EntityBrain.SetRigidbody(Rigidbody);
            m_EntityBrain.UltimateAction.OnUltimateActionExecuted += OnUltimateExecuted;
            m_EntityBrain.UltimateAction.OnFullyCharged += OnUltimateFullyCharged;
            m_EntityBrain.OnAfterDeath += OnTankDeath;
            m_EntityBrain.OnEntityVisibilityToggled += m_View.ToggleVisibility;
            m_EntityHealthController.OnHealthUpdated += m_View.UpdateHealthUI;
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


        private void OnUltimateFullyCharged() => IsUltimateReady = true;
        private void OnUltimateExecuted()
        {
            IsUltimateReady = false;
            m_StateMachine.OnUltimateExecuted();
        }

        private void OnTankDeath()
        {
            m_EntityBrain.UltimateAction.OnFullyCharged -= OnUltimateFullyCharged;
            m_EntityBrain.OnAfterDeath -= OnTankDeath;
            m_EntityBrain.OnEntityVisibilityToggled -= m_View.ToggleVisibility;
            m_EntityHealthController.OnHealthUpdated -= m_View.UpdateHealthUI;

            m_StateMachine.OnDeath();

            m_EntityBrain = null;
            m_Pool.ReturnEnemy(this);

            m_Service.OnEnemyDeath();
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
