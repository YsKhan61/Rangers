namespace BTG.Entity
{
    public interface INetworkEntityController
    {
        /// <summary>
        /// Get the owner client id of the entity
        /// </summary>
        public ulong OwnerClientId { get; }
    }
}