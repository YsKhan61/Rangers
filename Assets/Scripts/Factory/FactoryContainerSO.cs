using BTG.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BTG.Factory
{
    public abstract class FactoryContainerSO<T> : ScriptableObject
        where T : IFactoryItem
    {
        [SerializeField, Tooltip("The factories to create the items")]
        List<FactorySO<T>> m_Factories;

        /// <summary>
        /// Get the item from the factory based on the item tag
        /// </summary>
        public T GetItem(TagSO tag)
        {
            foreach (var factory in m_Factories)
            {
                if (factory.Tag == tag)
                {
                    return factory.GetItem();
                }
            }

            Debug.LogError("No factory found for the tag: " + tag.name);
            return default;
        }
    }
}
