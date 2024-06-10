using UnityEngine;

namespace BTG.Utilities
{
    public abstract class DoubleGenericEventChannelSO<T> : ScriptableObject
    {
        public event System.Action<T, T> OnEventRaised;

        public void RaiseEvent(T value1, T value2)
        {
            OnEventRaised?.Invoke(value1, value2);
        }
    }
}