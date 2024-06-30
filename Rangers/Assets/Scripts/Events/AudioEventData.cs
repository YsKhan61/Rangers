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
        public TagSO AudioTag;

        /// <summary>
        /// The parent of the audio clip.
        /// If null, the audio clip will be played at the Position.
        /// </summary>
        public Transform FollowTarget;

        /// <summary>
        /// Position where the audio clip will be played
        /// </summary>
        public Vector3 Position;
    }

}