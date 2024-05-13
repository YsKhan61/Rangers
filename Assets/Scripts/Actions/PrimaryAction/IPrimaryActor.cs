using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// An interface for the primary actor
    /// </summary>
    public interface IPrimaryActor :ITransform
    {
        /// <summary>
        /// Is the actor a player
        /// </summary>
        public bool IsPlayer { get; }

        /// <summary>
        /// The fire point of the actor
        /// </summary>
        public Transform FirePoint { get; }

        /// <summary>
        /// Current move speed of the actor
        /// </summary>
        public float CurrentMoveSpeed { get; }

        /// <summary>
        /// Get the primary action
        /// </summary>
        public IPrimaryAction PrimaryAction { get; }

        /// <summary>
        /// Start the primary fire
        /// </summary>
        public void StartPrimaryFire();

        /// <summary>
        /// Stop the primary fire
        /// </summary>
        public void StopPrimaryFire();
    }
}