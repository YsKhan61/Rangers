using BTG.Utilities;
using UnityEngine;

namespace BTG.Entity
{
    /// <summary>
    /// The entity factory scriptable object.
    /// Any entity factory should inherit from this class
    /// </summary>
    public abstract class EntityFactorySO : ScriptableObject
    {
        /// <summary>
        /// Get the entity from the factory
        /// </summary>
        /// <returns></returns>
        public abstract IEntityBrain GetEntity();

        /// <summary>
        /// Return the entity to the factory
        /// </summary>
        /// <param name="entity">the entity to return</param>
        public abstract void ReturnEntity(IEntityBrain entity);
    }
}


