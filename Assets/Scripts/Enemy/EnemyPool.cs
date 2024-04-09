using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Enemy
{ 
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private Transform m_EnemyContainer;
        public Transform EnemyContainer => m_EnemyContainer;

        [Inject]
        private EnemyDataSO m_Data;

        // public EnemyPool(EnemyDataSO data)
        public EnemyPool()
        {
            m_EnemyContainer = new GameObject("EnemyContainer").transform;
            // m_Data = data;
        }

        public EnemyController GetEnemy() => GetItem();

        public void ReturnEnemy(EnemyController enemy) => ReturnItem(enemy);

        protected override EnemyController CreateItem() => new EnemyController(m_Data, this);
    }
}
