using BTG.Factory;
using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "PrimaryActionFactoryContainer", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/PrimaryActionFactoryContainer")]
    public class PrimaryActionFactoryContainerSO : FactoryContainerSO<IPrimaryAction>
    {
    }
}