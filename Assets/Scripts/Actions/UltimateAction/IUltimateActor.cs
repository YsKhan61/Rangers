using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An interface for an entity that can execute an ultimate action.
    /// </summary>
    public interface IUltimateActor : ITransform
    {
        /// <summary>
        /// Is the actor a player?
        /// </summary>
        public bool IsPlayer { get; }

        /// <summary>
        /// Fire point of the actor. Can be used to instantiate projectiles.
        /// </summary>
        public Transform FirePoint { get; }

        /// <summary>
        /// What layer mask should be used to detect opposition?
        /// </summary>
        public LayerMask OppositionLayerMask { get; }

        /// <summary>
        /// Get the ultimate action of the actor.
        /// </summary>
        public IUltimateAction UltimateAction { get; }

        /// <summary>
        /// Get the damage collider of the actor.
        /// </summary>
        public IDamageable Damageable { get; }

        /// <summary>
        /// Try to execute the ultimate action.
        /// </summary>
        public void TryExecuteUltimate();

        /// <summary>
        /// Whether to show or hide the tank.
        /// Also mutes or unmutes the audio.
        /// </summary>
        /// <param name="value">true - show, unmute audio</param>
        public void ToggleActorVisibility(bool value);
    }
}