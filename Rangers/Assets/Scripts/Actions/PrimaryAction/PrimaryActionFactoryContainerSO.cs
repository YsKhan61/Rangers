using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// This concreate class is used to create asset of the primary action factory container,
    /// as the factory container is a generic class and can't be created as an asset.
    /// </summary>
    [CreateAssetMenu(fileName = "PrimaryActionFactoryContainer", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/PrimaryActionFactoryContainer")]
    public class PrimaryActionFactoryContainerSO : FactoryContainerSO<IPrimaryAction>
    {
    }
}