using BTG.Utilities;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace BTG.Enemy
{ 
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        [Inject]
        private EnemyDataSO m_Data;

        [Inject]
        private IObjectResolver m_Resolver;

        public EnemyController GetEnemy() => GetItem();

        public void ReturnEnemy(EnemyController enemy) => ReturnItem(enemy);

        protected override EnemyController CreateItem()
        {
            EnemyView view = Object.Instantiate(m_Data.EnemyPrefab);
            NavMeshAgent agent = (NavMeshAgent)view.gameObject.GetOrAddComponent<NavMeshAgent>();
            EnemyTankStateMachine stateMachine = new EnemyTankStateMachine();

            EnemyController controller = new EnemyController.Builder()
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
