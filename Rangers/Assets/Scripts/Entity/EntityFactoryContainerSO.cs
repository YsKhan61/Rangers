using BTG.Factory;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Entity
{
    [CreateAssetMenu(fileName = "EntityFactoryContainer", menuName = "ScriptableObjects/Factory/EntityFactory/EntityFactoryContainerSO")]
    public class EntityFactoryContainerSO : FactoryContainerSO<IEntityBrain>
    {
        public EntityFactorySO GetEntityFactory(TagSO tag) => GetFactory(tag) as EntityFactorySO;
    }
}


