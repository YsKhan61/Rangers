using BTG.Utilities;
using UnityEngine;

namespace BTG.Effects
{
    public abstract class EffectDataSO : ScriptableObject
    {
        [SerializeField]
        private TagSO m_Tag;
        public TagSO Tag => m_Tag;

        [SerializeField]
        private bool m_HasAudio;
        public bool HasAudio => m_HasAudio;

        [SerializeField]
        private int m_Duration;
        public int Duration => m_Duration;
    }

}
