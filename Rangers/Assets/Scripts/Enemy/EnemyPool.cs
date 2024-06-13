using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace BTG.Enemy
{ 
    public class EnemyPool : GenericObjectPool<EnemyTankController>
    {
        private Transform m_EnemyContainer;
        public Transform EnemyContainer => m_EnemyContainer;

        [Inject]
        private EnemyDataSO m_Data;

        [Inject]
        private IObjectResolver m_Resolver;

        public EnemyPool()
        {
            m_EnemyContainer = new GameObject("EnemyContainer").transform;
        }

        public EnemyTankController GetEnemy() => GetItem();

        public void ReturnEnemy(EnemyTankController enemy) => ReturnItem(enemy);

        protected override EnemyTankController CreateItem()
        {
            EnemyView view = Object.Instantiate(m_Data.EnemyPrefab, m_EnemyContainer);
            NavMeshAgent agent = (NavMeshAgent)view.gameObject.GetOrAddComponent<NavMeshAgent>();
            EnemyTankStateMachine stateMachine = new EnemyTankStateMachine();

            EnemyTankController controller = new EnemyTankController.Builder()
                .WithEnemyData(m_Data)
                .WithEnemyPool(this)
                .WithEnemyView(view)
                .WithNavMeshAgent(agent)
                .WithStateMachine(stateMachine)
                .Build();

            view.SetController(controller);
            stateMachine.SetController(controller);
            m_Resolver.Inject(stateMachine);
            controller.SetMaxLinearVelocity(m_Data.MaxSpeedMultiplier * m_Data.MaxSpeedMultiplier);
            m_Resolver.Inject(controller);

            return controller;
        }
    }

}
