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
        /// Get the owner of the object.
        /// </summary>
        public Transform Owner { get; }

        /// <summary>
        /// Is the object a player.
        /// </summary>
        public bool IsPlayer { get; }

        /// <summary>
        /// Is the object visible.
        /// </summary>
        public bool IsVisible { get; }

        /// <summary>
        /// Can the object take damage.
        /// </summary>
        public bool CanTakeDamage { get; }

        /// <summary>
        /// Damage the object.
        /// </summary>
        public void Damage(int damage);

        public void Damage(ulong actorOwnerClientId, int damage);

        /// <summary>
        /// Set whether the object is visible.
        /// </summary>
        public void SetVisible(bool visible);
    }
}