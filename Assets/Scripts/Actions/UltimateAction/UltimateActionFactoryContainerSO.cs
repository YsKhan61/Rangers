using BTG.Utilities;
using System.Collections.Generic;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An container for the ultimate action factories
    /// </summary>
    [CreateAssetMenu(fileName = "UltimateActionFactoryContainer", menuName = "ScriptableObjects/Factory/UltimateActionFactory/UltimateActionFactoryContainerSO")]
    public class UltimateActionFactoryContainerSO : ScriptableObject
    {
        [SerializeField]
        List<UltimateActionFactorySO> m_UltimateActionFactories;

        public IUltimateAction GetUltimateAction(IUltimateActor actor, TagSO tag)
        {
            foreach (var factory in m_UltimateActionFactories)
            {
                if (factory.UltimateTag == tag)
                {
                    return factory.CreateUltimateAction(actor);
                }
            }

            Debug.Log("No factory found for the tag: " + tag.name);
            return null;
        }
    }
}
