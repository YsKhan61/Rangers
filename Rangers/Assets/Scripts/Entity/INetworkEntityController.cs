namespace BTG.Entity
{
    /// <summary>
    /// This interface is used to define the controller of a network entity
    /// </summary>
    public interface INetworkEntityController
    {
        /// <summary>
        /// Get the owner client id of the entity
        /// </summary>
        public ulong OwnerClientId { get; }
    }
}