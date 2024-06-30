using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.Events
{
    public struct EntityInitializeEventData : IEvent 
    {
        public Sprite Sprite;
    }
}