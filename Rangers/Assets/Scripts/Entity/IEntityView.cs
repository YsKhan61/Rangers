using UnityEngine;

namespace BTG.Entity
{
    /// <summary>
    /// An interface for all entity views of the game
    /// </summary>
    public interface IEntityView
    { 
        public Transform CameraTarget { get; }
    }

}
