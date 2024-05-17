using BTG.Utilities;
using UnityEngine;

namespace BTG.Enemy
{
    [CreateAssetMenu(fileName = "EnemyTankUltimateStateFactoryContainer", menuName = "ScriptableObjects/Factory/EnemyStateFactory/EnemyTankUltimateStateFactoryContainerSO")]
    public class EnemyTankUltimateStateFactoryContainerSO : ScriptableObject
    {
        [SerializeField, Tooltip("The factories for the ultimate states")]
        private EnemyTankUltimateStateFactorySO[] m_Factories;

        /// <summary>
        /// Get the factory for the ultimate state
        /// </summary>
        /// <param name="tag">tag for the ultimate state</param>
        /// <returns>the factory</returns>
        public EnemyTankUltimateStateFactorySO GetFactory(TagSO tag)
        {
            foreach (var factory in m_Factories)
            {
                if (factory.UltimateActionTag == tag)
                {
                    return factory;
                }
            }

            return null;
        }
    }
}
