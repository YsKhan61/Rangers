using BTG.Factory;
using UnityEngine;


namespace BTG.Effects
{
    /// <summary>
    /// Any effect that is to be shown in the game should inherit from this class.
    /// All effects should be created by factory, hence this class is derived from IFactoryItem.
    /// </summary>
    public abstract class EffectView : MonoBehaviour, IFactoryItem
    {
        public abstract void Play();
    }
}