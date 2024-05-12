using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using System;
using UnityEngine;


namespace BTG.Entity
{
    /// <summary>
    /// An interface for the tank brain.
    /// Player and enemy tank controllers can have references to this interface.
    /// This promotes loose coupling between the tank brain and the tank controllers (playertankcontroller, enemytankcontroller).
    /// </summary>
    public interface IEntityTankBrain : IEntityBrain, IPrimaryActor, IUltimateActor
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action OnAfterDeath;
        public event Action<bool> OnEntityVisibilityToggled;

        /// <summary>
        /// Get the model data of the tank.
        /// </summary>
        public IEntityTankModel Model { get; }

        /// <summary>
        /// Get the transform where the camera will be assigned to
        /// </summary>
        public Transform CameraTarget { get; }

        /// <summary>
        /// Get the health controller of the tank.
        /// </summary>
        public IEntityHealthController HealthController { get; }

        /// <summary>
        /// Set the self and opposition layers of the tank.
        /// This layers helps the tank to identify the opposition tanks.
        /// </summary>
        public void SetLayers(int selfLayer, int oppositionLayer);

        /// <summary>
        /// Set the parent of the tank view.
        /// It is used to set the parent of the tank view to the tank container or Player/Enemy View
        /// </summary>
        public void SetParentOfView(Transform parent, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Set the rigidbody of the tank.
        /// </summary>
        public void SetRigidbody(Rigidbody rb);
    }
}
