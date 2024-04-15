using System;

namespace BTG.Utilities.EventBus
{
    internal interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }

    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        private Action<T> m_OnEvent = _ => { }; // delegate(T _) { };
        private Action m_OnEventNoArgs = () => { }; // delegate { };

        Action<T> IEventBinding<T>.OnEvent
        { 
            get => m_OnEvent;
            set => m_OnEvent = value;
        }

        Action IEventBinding<T>.OnEventNoArgs
        {
            get => m_OnEventNoArgs;
            set => m_OnEventNoArgs = value;
        }

        public EventBinding (Action<T> onEvent) => m_OnEvent = onEvent;
        public EventBinding (Action onEvent) => m_OnEventNoArgs = onEvent;

        public void Add(Action<T> action) => m_OnEvent += action;
        public void Add(Action action) => m_OnEventNoArgs += action;
        public void Remove(Action<T> action) => m_OnEvent -= action;
        public void Remove(Action action) => m_OnEventNoArgs -= action;
    }
}