using BTG.Events;
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
        /// Is the actor a network player?
        /// </summary>
        public bool IsNetworkPlayer { get; }

        /// <summary>
        /// Get the owner client id of the actor
        /// </summary>
        public ulong OwnerClientId { get; }

        /// <summary>
        /// The network object id of the actor, if it is a network player
        /// </summary>
        public ulong NetworkObjectId { get; }

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
        public void StartPrimaryAction();

        /// <summary>
        /// Stop the primary fire
        /// </summary>
        public void StopPrimaryAction();

        /// <summary>
        /// Automatically start the primary action and stop it after certain time
        /// </summary>
        public void AutoStartStopPrimaryAction(int stopTime);

        public void RaisePlayerCamShakeEvent(CameraShakeEventData camShakeData);
    }
}