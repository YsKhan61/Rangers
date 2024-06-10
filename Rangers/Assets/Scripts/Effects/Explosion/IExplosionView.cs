using BTG.Factory;

namespace BTG.Effects
{
    /// <summary>
    /// An interface for the explosion view of the game
    /// It is used to play the explosion effect
    /// It can be created by factory as it implements the IFactoryItem interface
    /// </summary>
    public interface IExplosionView : IFactoryItem
    {
        /// <summary>
        /// Play the explosion effect
        /// </summary>
        public void Play();
    }

}
