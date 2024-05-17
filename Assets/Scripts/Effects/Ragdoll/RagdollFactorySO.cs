using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Factory", menuName = "ScriptableObjects/Factory/Effects Factory/RagdollFactorySO")]
    public class RagdollFactorySO : ScriptableObject
    {
        [SerializeField]
        private RagdollDataSO m_Data;
        public RagdollDataSO Data => m_Data;

        private RagdollPool m_Pool;
        private RagdollPool Pool => m_Pool ??= new RagdollPool(m_Data);

        /// <summary>
        /// Get a ragdoll from the pool and set the owner
        /// </summary>
        public RagdollView GetRagdoll() => Pool.GetRagdoll();
    }
}