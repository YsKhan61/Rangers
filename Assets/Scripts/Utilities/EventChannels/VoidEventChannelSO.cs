using UnityEngine;

namespace BTG.Utilities
{
    [CreateAssetMenu(fileName = "VoidEventChannel", menuName = "ScriptableObjects/EventChannels/VoidEventChannelSO")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event System.Action OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}