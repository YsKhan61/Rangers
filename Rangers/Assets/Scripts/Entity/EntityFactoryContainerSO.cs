using BTG.Factory;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Entity
{
    [CreateAssetMenu(fileName = "EntityFactoryContainer", menuName = "ScriptableObjects/Factory/EntityFactory/EntityFactoryContainerSO")]
    public class EntityFactoryContainerSO : FactoryContainerSO<IEntityBrain>
    {
        // public IEntityBrain GetEntity(TagSO tag) => GetItem(tag);
    }
}


