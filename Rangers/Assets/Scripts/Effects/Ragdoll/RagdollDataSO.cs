using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    [CreateAssetMenu(fileName = "Ragdoll Data", menuName = "ScriptableObjects/Effects/RagdollDataSO")]
    public class RagdollDataSO : ScriptableObject
    {
        [SerializeField, Tooltip("The tag of the ragdoll that will be created, should be same as owner!")]
        private TagSO m_Tag;
        public TagSO Tag => m_Tag;

        [SerializeField, Tooltip("The prefab of the ragdoll")]
        private RagdollView m_RagdollPrefab;
        public RagdollView RagdollPrefab => m_RagdollPrefab;

        [SerializeField, Tooltip("The audio clip to be played with this effect")]
        private AudioClip m_AudioClip;
        public AudioClip AudioClip => m_AudioClip;
    }
}