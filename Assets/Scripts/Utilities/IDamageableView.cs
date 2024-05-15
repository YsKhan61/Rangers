using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An interface for an object that can take damage.
    /// </summary>
    public interface IDamageableView
    {
        /// <summary>
        /// Get the transform of the object.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// Damage the object.
        /// </summary>
        /// <param name="damage"></param>
        public void Damage(int damage);
    }
}