using BTG.Utilities.EventBus;

namespace BTG.Events
{
    public struct CameraShakeEventData : IEvent 
    {
        public float ShakeAmount;
        public float ShakeDuration;
    }
}