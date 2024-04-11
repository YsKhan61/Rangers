using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Enemy
{ 
    public class EnemyPool : GenericObjectPool<EnemyTankController>
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

        public EnemyTankController GetEnemy() => GetItem();

        public void ReturnEnemy(EnemyTankController enemy) => ReturnItem(enemy);

        protected override EnemyTankController CreateItem() => new(m_Data, this);
    }
}
