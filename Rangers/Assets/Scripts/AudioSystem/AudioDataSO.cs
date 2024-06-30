using BTG.Utilities;
using UnityEngine;


namespace BTG.AudioSystem
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData")]
    public class AudioDataSO : ScriptableObject
    {
        [SerializeField]
        private TagSO m_Tag;
        public TagSO Tag => m_Tag;

        [SerializeField]
        private AudioClip m_Clip;
        public AudioClip Clip => m_Clip;

        [SerializeField]
        private bool m_Loop;
        public bool Loop => m_Loop;

        [SerializeField]
        private float m_SpatialBlend;
        public float SpatialBlend => m_SpatialBlend;

        [SerializeField]
        private int m_Duration;
        public int Duration => m_Duration;
    }
}
