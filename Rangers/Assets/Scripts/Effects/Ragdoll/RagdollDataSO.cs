using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Data", menuName = "ScriptableObjects/Effects/RagdollDataSO")]
    public class RagdollDataSO : EffectDataSO
    {
        [SerializeField, Tooltip("The prefab of the ragdoll")]
        private RagdollView m_RagdollPrefab;
        public RagdollView RagdollPrefab => m_RagdollPrefab;
    }
}