using UnityEngine;

namespace BTG.Utilities
{
    [CreateAssetMenu(fileName = "IntIntEventChannel", menuName = "ScriptableObjects/EventChannels/IntIntEventChannelSO")]
    public class IntIntEventChannelSO : ScriptableObject
    {
        public event System.Action<int, int> OnPlayerHealthUpdated;

        public void RaiseEvent(int currentHealth, int maxHealth)
        {
            OnPlayerHealthUpdated?.Invoke(currentHealth, maxHealth);
        }
    }
}