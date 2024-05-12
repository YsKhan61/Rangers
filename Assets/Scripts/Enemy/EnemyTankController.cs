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

        private EnemyStateManager m_StateManager;
        public Rigidbody Rigidbody => m_View.Rigidbody;
        public Transform Transform => m_View.transform;

        public IPlayerView TargetView { get; private set; }
        public bool IsTargetInRange  => TargetView != null;


        public EnemyTankController(EnemyDataSO data, EnemyPool pool)
        {
            m_Pool = pool;
            m_Data = data;
            m_View = Object.Instantiate(m_Data.EnemyPrefab, pool.EnemyContainer);
            m_View.SetController(this);
            m_Agent = m_View.GetComponent<NavMeshAgent>();
            m_StateManager = new EnemyStateManager(this);

            Rigidbody.maxLinearVelocity = m_Data.MaxSpeedMultiplier * m_Data.MaxSpeedMultiplier;
        }

        ~EnemyTankController()
        {
            m_EntityBrain.OnAfterDeath -= OnTankDeath;
            m_EntityBrain.OnEntityVisibilityToggled -= m_View.ToggleVisibility;
            m_EntityBrain.UltimateAction.OnFullyCharged -= ExecuteUltimate;
            m_EntityHealthController.OnHealthUpdated -= m_View.UpdateHealthUI;
            m_EntityBrain = null;
        }

        public void Init()
        {
            m_Agent.speed = m_EntityBrain.Model.MaxSpeed * m_Data.MaxSpeedMultiplier;
            m_Agent.stoppingDistance = m_Data.StoppingDistance;

#if UNITY_EDITOR
            if (!m_Data.InitializeState)
            {
                // This is to prevent the state from being initialized in the editor if it is not needed
                return;
            }
#endif

            m_StateManager.Init();
            m_StateManager.ChangeState(m_Data.InitialState);
        }

        public void SetPose(in Pose pose) => m_View.transform.SetPose(pose);

        /// <summary>
        /// Set the player view that has been detected
        /// </summary>
        public void SetPlayerView(IPlayerView view) => TargetView = view;

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
            m_EntityBrain.UltimateAction.OnFullyCharged += ExecuteUltimate;
            m_EntityBrain.OnAfterDeath += OnTankDeath;
            m_EntityBrain.OnEntityVisibilityToggled += m_View.ToggleVisibility;
            m_EntityHealthController.OnHealthUpdated += m_View.UpdateHealthUI;
            m_EntityBrain.Init();
        }


        public void SetService(EnemyService service) => m_Service = service;

        public void ExecutePrimaryAction() => m_EntityBrain.StartPrimaryFire();

        public void ExecuteUltimate() => m_EntityBrain.TryExecuteUltimate();


        private void OnTankDeath()
        {
            m_EntityBrain.UltimateAction.OnFullyCharged -= ExecuteUltimate;
            m_EntityBrain.OnAfterDeath -= OnTankDeath;
            m_EntityBrain.OnEntityVisibilityToggled -= m_View.ToggleVisibility;
            m_EntityHealthController.OnHealthUpdated -= m_View.UpdateHealthUI;

            m_StateManager.ChangeState(EnemyState.Dead);
            m_StateManager.DeInit();

            m_EntityBrain = null;
            m_Pool.ReturnEnemy(this);

            m_Service.OnEnemyDeath();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Transform.position, m_Agent.destination);
        }
    }
}
