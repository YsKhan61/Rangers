using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An container for the ultimate action factories
    /// This concrete class is needed to create a scriptable object assets in the project,
    /// as Unity does not support creating scriptable object assets from generic classes. (FactoryContainerSO<T>)
    /// </summary>
    [CreateAssetMenu(fileName = "UltimateActionFactoryContainer", menuName = "ScriptableObjects/Factory/UltimateActionFactory/UltimateActionFactoryContainerSO")]
    public class UltimateActionFactoryContainerSO : FactoryContainerSO<IUltimateAction>
    {
    }
}
