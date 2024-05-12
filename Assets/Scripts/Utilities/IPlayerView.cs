using UnityEngine;

namespace BTG.Utilities
{
    /// <summary>
    /// An interface for the player view
    /// Any class that implements this interface is considered a player view
    /// It subtitues the direct reference to the player view in other classes
    /// Enemy classes can scan for the player view without knowing the exact type of the player view
    /// </summary>
    public interface IPlayerView
    {
        public Transform Transform { get; }
    }
}