using BTG.Utilities;

namespace BTG.Entity
{
    /// <summary>
    /// An interface for the tank model
    /// </summary>
    public interface IEntityTankModel
    {
        /// <summary>
        /// Is the model a player
        /// </summary>
        public bool IsPlayer { get; set; }

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