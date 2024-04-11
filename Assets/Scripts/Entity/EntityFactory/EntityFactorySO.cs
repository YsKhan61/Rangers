using BTG.Utilities;
using UnityEngine;

namespace BTG.Entity
{
    public abstract class EntityFactorySO : ScriptableObject
    {
        public abstract IEntityBrain GetEntity();
        public abstract void ReturnEntity(IEntityBrain entity);
    }
}


