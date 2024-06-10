using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An interface for an object that can be fired.
    /// Projectiles, bullets, etc. can implement this interface.
    /// </summary>
    public interface  IFiringView
    {
        /// <summary>
        /// The owner of the firing object.
        /// </summary>
        public Transform Owner { get; }
    }
}