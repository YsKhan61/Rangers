using BTG.Utilities;

namespace BTG.Effects
{
    /// <summary>
    /// An interface for the owner of the ragdoll
    /// </summary>
    public interface  IRagdollOwner : ITransform
    {
        /// <summary>
        /// Get the tag of the owner.
        /// </summary>
        public TagSO Tag { get; }

        /// <summary>
        /// Execute the ragdoll effect.
        /// </summary>
        public void ExecuteRagdollEffect();
    }
}