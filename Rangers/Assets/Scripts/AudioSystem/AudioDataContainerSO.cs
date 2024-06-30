using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.AudioSystem
{
    [CreateAssetMenu(fileName = "AudioDataContainer", menuName = "ScriptableObjects/AudioDataContainerSO")]
    public class AudioDataContainerSO : ScriptableObject
    {
        [SerializeField]
        private AudioDataSO[] m_AudioDatas;

        public bool TryGetAudioData(TagSO tag, out AudioDataSO audioData)
        {
            audioData = null;
            foreach (var data in m_AudioDatas)
            {
                if (data.Tag == tag)
                {
                    audioData = data;
                    return true;
                }
            }

            Debug.Assert(audioData != null, $"AudioData with tag {tag} not found.");
            return false;
        }


        public bool TryGetAudioData(Guid guid, out AudioDataSO audioData)
        {
            audioData = null;
            foreach (var data in m_AudioDatas)
            {
                if (data.Tag.Guid == guid)
                {
                    audioData = data;
                    return true;
                }
            }

            Debug.Assert(audioData != null, $"AudioData with guid {guid} not found.");
            return false;
        }
    }
}
