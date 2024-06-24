using BTG.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace BTG.Factory
{
    /// <summary>
    /// A generic factory container that holds the factories to create the items
    /// The factories are created based on the item tag and of the type T(item type)
    /// The item types must implement the IFactoryItem interface
    /// </summary>
    /// <typeparam name="T">Type of the item that will be created</typeparam>
    public abstract class FactoryContainerSO<T> : ScriptableObject
        where T : IFactoryItem
    {
        [SerializeField, Tooltip("The factories to create the items")]
        List<FactorySO<T>> m_Factories;

        [Inject]
        public void InjectIntoFactories(IObjectResolver resolver)
        {
            if (m_Factories == null)
            {
                Debug.Log("The factories are not set in the factory container: " + name);
                return;
            }

            foreach (var factory in m_Factories)
            {
                resolver.Inject(factory);
            }
        }

        public FactorySO<T> GetFactory(TagSO tag)
        {
            foreach (var factory in m_Factories)
            {
                if (factory.Tag == tag)
                {
                    return factory;
                }
            }

            Debug.LogError("No factory found for the tag: " + tag.name);
            return default;
        }

        /// <summary>
        /// For this method to work, the factory must have a Tag property with a Guid
        /// </summary>
        public FactorySO<T> GetFactory(Guid guid)
        {
            foreach (var factory in m_Factories)
            {
                if (factory.Tag.Guid == guid)
                {
                    return factory;
                }
            }

            Debug.LogError("No factory found for the guid: " + guid);
            return default;
        }
    }
}
