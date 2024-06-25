using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.Events
{
    /// <summary>
    /// In singleplayer - An event data struct that holds the data for an effect event.
    /// </summary>
    public struct EffectEventData : IEvent
    {
        /// <summary>
        /// The parent of the effect. If null, the effect will be spawned at the EffectPosition.
        /// </summary>
        public Transform FollowTarget;

        /// <summary>
        /// The tag of the effect.
        /// </summary>
        public TagSO EffectTag;

        /// <summary>
        /// If the effect dont have a parent, this is the position where the effect will be spawned.
        /// </summary>
        public Vector3 EffectPosition;

        /// <summary>
        /// Duration of the effect.
        /// </summary>
        public int Duration;
    }
}