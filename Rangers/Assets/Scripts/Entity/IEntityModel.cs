using BTG.Utilities;

namespace BTG.Entity
{
    /// <summary>
    /// An interface for the tank model
    /// </summary>
    public interface IEntityModel
    {
        /// <summary>
        /// Is the model a player
        /// </summary>
        public bool IsPlayer { get; set; }

        /// <summary>
        /// Is the model a network player
        /// </summary>
        public bool IsNetworkPlayer { get; set; }

        /// <summary>
        /// Get the owner client id of the Entity
        /// </summary>
        public ulong OwnerClientId { get; set; }

        /// <summary>
        /// The network object id of the actor, if it is a network player
        /// </summary>
        public ulong NetworkObjectId { get; set; }

        /// <summary>
        /// Get the maximum health of the tank
        /// </summary>
        public int MaxHealth { get; }

        /// <summary>
        /// Get the maximum speed of the tank
        /// </summary>
        public int MaxSpeed { get; }

        /// <summary>
        /// Get the rotation speed of the tank
        /// </summary>
        public int RotateSpeed { get; }

        /// <summary>
        /// Get the acceleration of the tank
        /// </summary>
        public float Acceleration { get; }

        /// <summary>
        /// Get the ultimate action tag of the tank
        /// </summary>
        public TagSO UltimateTag { get; }

    }

}