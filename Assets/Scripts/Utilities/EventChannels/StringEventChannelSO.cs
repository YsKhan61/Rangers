using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An event channel that has a string as a parameter
    /// </summary>
    [CreateAssetMenu(fileName = "StringEventChannel", menuName = "ScriptableObjects/EventChannels/StringEventChannelSO")]
    public class StringEventChannelSO : GenericEventChannelSO<string>
    {
    }
}