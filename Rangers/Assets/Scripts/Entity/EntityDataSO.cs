using BTG.Utilities;
using UnityEngine;

namespace BTG.Entity
{
    /// <summary>
    /// An abstract class to hold data for an entity.
    /// All entity data scriptable objects should inherit from this class.
    /// </summary>
    public abstract class EntityDataSO : GuidSO
    {
        public virtual int CharSelectSeatIndex { get; }
    
        public virtual GameObject Graphics { get; }
    }
}
