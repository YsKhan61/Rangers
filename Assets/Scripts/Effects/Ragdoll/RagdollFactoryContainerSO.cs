using BTG.Factory;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Factory Container", menuName = "ScriptableObjects/Factory/Effects Factory/RagdollFactoryContainerSO")]
    public class RagdollFactoryContainerSO : FactoryContainerSO<RagdollView>
    {
        /// <summary>
        /// First get the ragdoll from the pool and then execute the ragdoll effect
        /// </summary>
        public void GetAndExecuteRagdollEffect(IRagdollOwner owner)
        {
            RagdollView view = GetRagdoll(owner.Tag);
            ExecuteRagdollEffect(view, owner);
        }

        /// <summary>
        /// Get the ragdoll factory by using entity tag
        /// </summary>
        public RagdollView GetRagdoll(TagSO tag) => GetItem(tag);

        /// <summary>
        /// Execute the ragdoll effect with the owner
        /// </summary>
        public void ExecuteRagdollEffect(RagdollView view, IRagdollOwner owner)
        {
            view.SetOwner(owner);
            view.Execute(new Pose(owner.Transform.position, owner.Transform.rotation));
        }
    }
}