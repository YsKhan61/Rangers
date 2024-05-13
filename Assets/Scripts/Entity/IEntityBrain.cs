using BTG.Utilities;
using UnityEngine;


namespace BTG.Entity
{
    /// <summary>
    /// An interface for the brain of an entity.
    /// Any entity of the game should have a brain that implements this interface.
    /// </summary>
    public interface IEntityBrain : ITransform
    {
        /// <summary>
        /// Initialize the brain
        /// It should be called after the brain is created or gotten from the pool
        /// </summary>
        public void Init();

        /// <summary>
        /// De-initialize the brain.
        /// </summary>
        public void DeInit();
    }

}
