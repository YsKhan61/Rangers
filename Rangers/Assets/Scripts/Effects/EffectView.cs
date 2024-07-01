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
        protected int overridenDuration;

        public abstract void Play();

        /// <summary>
        /// This method overrides the default duration of the effect data
        /// </summary>
        public void SetDuration(int duration) => this.overridenDuration = duration;
    }
}