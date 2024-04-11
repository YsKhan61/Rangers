using BTG.Utilities;
using UnityEngine;

namespace BTG.Entity
{
    [System.Serializable]
    public class EntityFactoryData
    {
        public int EntityID;
        public TagSO EntityTag;
        public EntityFactorySO EntityFactory;
    }


    [CreateAssetMenu(fileName = "EntityFactoryContainer", menuName = "ScriptableObjects/EntityFactory/EntityFactoryContainerSO")]
    public class EntityFactoryContainerSO : ScriptableObject
    {
        public EntityFactoryData[] EntityFactories;

        public bool TryGetFactory(TagSO EntityTag, out EntityFactorySO factory)
        {
            factory = null;

            for (int i = 0, count = EntityFactories.Length; i < count; i++)
            {
                if (EntityFactories[i].EntityTag == EntityTag)
                {
                    factory = EntityFactories[i].EntityFactory;
                    return true;
                }
            }

            return false;
        }

        public bool TryGetFactory(int id, out EntityFactorySO factory)
        {
            factory = null;

            for(int i = 0, count = EntityFactories.Length; i < count; i++)
            {
                if (EntityFactories[i].EntityID == id)
                {
                    factory = EntityFactories[i].EntityFactory;
                    return true;
                }
            }

            return false;
        }
    }
}


