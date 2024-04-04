using BTG.Utilities;
using UnityEngine;

namespace BTG.Enemy
{ 
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private EnemyView m_EnemyPrefab;
        private Transform m_EnemyContainer;
        public Transform EnemyContainer => m_EnemyContainer;

        public EnemyPool(EnemyView enemyPrefab)
        {
            m_EnemyContainer = new GameObject("EnemyContainer").transform;
            m_EnemyPrefab = enemyPrefab;
        }

        public EnemyController GetEnemy()
        {
            EnemyController enemy = GetItem();
            return enemy;
        }

        public void ReturnEnemy(EnemyController enemy) => ReturnItem(enemy);

        protected override EnemyController CreateItem() => new EnemyController(m_EnemyPrefab, this);
    }
}
