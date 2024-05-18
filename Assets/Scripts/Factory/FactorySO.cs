using BTG.Utilities;
using UnityEngine;

namespace BTG.Factory
{
    /// <summary>
    /// An abstract factory that creates the items of the project
    /// Any factory must inherit from this class
    /// The item types must implement the IFactoryItem interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FactorySO<T> : ScriptableObject
        where T : IFactoryItem
    {
        [SerializeField, Tooltip("Tag of item to be created")]
        TagSO m_Tag;

        /// <summary>
        /// Tag of item to be created
        /// </summary>
        public TagSO Tag => m_Tag;

        /// <summary>
        /// Get the item from factory of type that implements IFactoryItem
        /// </summary>
        public abstract T GetItem();
    }
}
