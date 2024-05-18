using BTG.Utilities;
using UnityEngine;

namespace BTG.Factory
{
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
        /// Get the item from factory
        /// </summary>
        public abstract T GetItem();
    }
}
