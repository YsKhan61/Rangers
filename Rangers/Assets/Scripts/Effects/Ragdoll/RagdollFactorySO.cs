using UnityEngine;


namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Factory", menuName = "ScriptableObjects/Factory/Effects Factory/RagdollFactorySO")]
    public class RagdollFactorySO : EffectFactorySO
    {
        [SerializeField]
        private RagdollDataSO m_Data;
        public override EffectDataSO Data => m_Data;

        private RagdollPool m_Pool;
        private RagdollPool Pool => m_Pool ??= new RagdollPool(m_Data);

        public override EffectView GetItem() => Pool.GetRagdoll();
    }
}