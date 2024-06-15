using UnityEngine;

namespace BTG.Player
{
    public interface IPlayerService
    {
        public void OnEntityInitialized(Sprite icon);
        public void OnPlayerDeath();
    }
}

