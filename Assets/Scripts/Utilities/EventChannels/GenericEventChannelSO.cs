using UnityEngine;

namespace BTG.Utilities
{
    public abstract class GenericEventChannelSO<T> : ScriptableObject
    {
        public event System.Action<T> OnEventRaised;

        public virtual void RaiseEvent(T value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}