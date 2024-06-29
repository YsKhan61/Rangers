using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.Events
{
    public struct AudioEventData : IEvent
    {
        /// <summary>
        /// Tag of the audio data
        /// </summary>
        public TagSO Tag;

        /// <summary>
        /// Position where the audio clip will be played
        /// </summary>
        public Vector3 Position;
    }
}