using System;
using UnityEngine;


namespace BTG.Utilities
{
    public abstract class GenericDataSO<T> : ScriptableObject where T : struct
    {
        private T m_Value;

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => m_Value;
            set
            {
                m_Value = value;
                OnValueChanged?.Invoke(m_Value);
            }
        }

        // Create implicit conversion from IntDataSO to int
        public static implicit operator T(GenericDataSO<T> data) => data.Value;
    }

}