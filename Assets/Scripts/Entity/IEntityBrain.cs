using BTG.Utilities;
using UnityEngine;


namespace BTG.Entity
{
    public interface IEntityBrain : ITransform
    {
        /// <summary>
        /// Initialize the brain
        /// It should be called after the brain is created or gotten from the pool
        /// </summary>
        public void Init();
    }

}
