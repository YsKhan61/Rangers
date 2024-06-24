using BTG.Utilities;
using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.Events
{
    public struct EffectEvent : IEvent
    {
        public TagSO EffectTag;
        public Vector3 EffectPosition;
    }
}