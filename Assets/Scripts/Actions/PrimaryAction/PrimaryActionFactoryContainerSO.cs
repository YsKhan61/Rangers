using BTG.Factory;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "PrimaryActionFactoryContainer", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/PrimaryActionFactoryContainer")]
    public class PrimaryActionFactoryContainerSO : FactoryContainerSO<IPrimaryAction>
    {
        /// <summary>
        /// Get the primary action from the factory based on the item tag
        /// </summary>
        public IPrimaryAction GetPrimaryAction(TagSO tag) => GetItem(tag);
    }
}