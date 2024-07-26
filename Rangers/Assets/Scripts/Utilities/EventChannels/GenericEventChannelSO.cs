using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An abstract scriptable object class for the generic event channel
    /// It contains the event that can be raised and the method to raise the event
    /// A scriptable object class of any type can inherit from this class to create an event channel
    /// </summary>
    /// <typeparam name="T">T is the type of the value that will be passed when the event is raised</typeparam>
    public abstract class GenericEventChannelSO<T> : ScriptableObject
    {
        /// <summary>
        /// Invoked when the event is raised
        /// It contains the value that is passed when the event is raised
        /// </summary>
        public event System.Action<T> OnEventRaised;

        /// <summary>
        /// Method to raise the event
        /// </summary>
        /// <param name="value">value to be passed through the event</param>
        public virtual void RaiseEvent(T value) => OnEventRaised?.Invoke(value);
    }
}