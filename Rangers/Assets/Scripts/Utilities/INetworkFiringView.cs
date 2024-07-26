namespace BTG.Utilities
{
    /// <summary>
    /// An interface for an network object that can be fired.
    /// </summary>
    public interface INetworkFiringView
    {
        /// <summary>
        /// Get the client id of the actor of the action, who fires this object.
        /// </summary>
        public ulong ActorOwnerClientId { get; }
    }
}