using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An event channel that has a TagSO as a parameter
    /// </summary>
    [CreateAssetMenu(fileName = "TagEventChannel", menuName = "ScriptableObjects/EventChannels/TagEventChannelSO")]
    public class TagEventChannelSO : GenericEventChannelSO<TagSO>
    {
    }
}