using BTG.Factory;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An container for the ultimate action factories
    /// </summary>
    [CreateAssetMenu(fileName = "UltimateActionFactoryContainer", menuName = "ScriptableObjects/Factory/UltimateActionFactory/UltimateActionFactoryContainerSO")]
    public class UltimateActionFactoryContainerSO : FactoryContainerSO<IUltimateAction>
    {
        public UltimateActionFactorySO GetUltimateActionFactory(TagSO tag) => GetFactory(tag) as UltimateActionFactorySO;
    }
}
