using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Factory Container", menuName = "ScriptableObjects/Factory/Effects Factory/RagdollFactoryContainerSO")]
    public class RagdollFactoryContainerSO : ScriptableObject
    {
        [SerializeField]
        private RagdollFactorySO[] m_Factories;

        /// <summary>
        /// Get the ragdoll factory by using entity tag
        /// </summary>
        public RagdollView GetRagdoll(TagSO tag)
        {
            if (TryGetFactory(tag, out RagdollFactorySO factory))
            {
                return factory.GetRagdoll();
            }

            return null;
        }

        /// <summary>
        /// Execute the ragdoll effect with the owner
        /// </summary>
        public void ExecuteRagdollEffect(RagdollView view, IRagdollOwner owner)
        {
            view.SetOwner(owner);
            view.Execute(new Pose(owner.Transform.position, owner.Transform.rotation));
        }

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
        public bool TryGetFactory(TagSO tag, out RagdollFactorySO factory)
        {
            foreach (var f in m_Factories)
            {
                if (f.Data.Tag == tag)
                {
                    factory = f;
                    return true;
                }
            }

            factory = null;
            return false;
        }
    }
}