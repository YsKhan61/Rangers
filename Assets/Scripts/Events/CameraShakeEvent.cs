using BTG.Utilities.EventBus;

namespace BTG.Events
{
    public struct CameraShakeEvent : IEvent 
    {
        public float ShakeAmount;
        public float ShakeDuration;
    }
}