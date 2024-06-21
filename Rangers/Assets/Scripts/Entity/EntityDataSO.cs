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
        [SerializeField, Tooltip("The tag of the tank")]
        private TagSO m_Tag;
        public TagSO Tag => m_Tag;

        // public virtual TagSO Tag { get; }
        public virtual Sprite Icon { get; }
        public virtual int CharSelectSeatIndex { get; }
        public virtual GameObject Graphics { get; }
        public virtual int MaxHealth { get; }
    }
}
