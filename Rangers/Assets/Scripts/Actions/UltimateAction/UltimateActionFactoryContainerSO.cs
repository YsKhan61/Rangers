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
        /*/// <summary>
        /// Get the ultimate action from the factory based on the item tag
        /// </summary>
        public IUltimateAction GetUltimateAction(TagSO tag) => GetItem(tag);*/
    }
}
