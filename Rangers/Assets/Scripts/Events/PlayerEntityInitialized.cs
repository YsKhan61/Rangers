using BTG.Utilities.EventBus;
using UnityEngine;

namespace BTG.Events
{
    public struct PlayerEntityInitialized : IEvent 
    {
        public Sprite Sprite;
    }
}