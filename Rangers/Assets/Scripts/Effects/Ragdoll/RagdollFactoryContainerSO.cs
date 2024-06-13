using BTG.Factory;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Factory Container", menuName = "ScriptableObjects/Factory/Effects Factory/RagdollFactoryContainerSO")]
    public class RagdollFactoryContainerSO : FactoryContainerSO<RagdollView>
    {
    }
}