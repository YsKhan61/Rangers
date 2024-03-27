using System;

namespace BTG.EventSystem
{
    public class EventController
    {
        public event Action OnEventInvoked;

        public void AddListener(Action action)
        {
            OnEventInvoked += action;
        }

        public void RemoveListener(Action action)
        {
            OnEventInvoked -= action;
        }

        public void InvokeEvent()
        {
            OnEventInvoked?.Invoke();
        }
    }

    public class EventController<T>
    { 
        public event Action<T> OnEventInvoked;

        public void AddListener(Action<T> action)
        {
            OnEventInvoked += action;
        }

        public void RemoveListener(Action<T> action)
        {
            OnEventInvoked -= action;
        }

        public void InvokeEvent(T arg)
        {
            OnEventInvoked?.Invoke(arg);
        }
    }
}
