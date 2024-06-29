using BTG.Actions.PrimaryAction;
using BTG.Actions.UltimateAction;
using BTG.Effects;
using BTG.Events;
using BTG.Factory;
using BTG.Utilities;
using System;
using UnityEngine;


namespace BTG.Entity
{
    /// <summary>
    /// An interface for the brain of an entity.
    /// Any entity of the game should have a brain that implements this interface.
    /// </summary>
    public interface IEntityBrain : ITransform, IFactoryItem, IPrimaryActor, IUltimateActor, IUpdatable, IDestroyable // IRagdollOwner,
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action<bool> OnEntityVisibilityToggled;
        public event Action<CameraShakeEventData> OnPlayerCameraShake;

        /// <summary>
        /// Get the model data of the tank.
        /// </summary>
        public IEntityModel Model { get; }

        /// <summary>
        /// Get the transform where the camera will be assigned to
        /// </summary>
        public Transform CameraTarget { get; }

        /// <summary>
        /// Get the damage collider of the tank.
        /// </summary>
        public Collider DamageCollider { get; }

        /// <summary>
        /// Set the parent of the tank view.
        /// It is used to set the parent of the tank view to the tank container or Player/Enemy View
        /// </summary>
        public void SetParentOfView(Transform parent, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Set the rigidbody of the tank.
        /// </summary>
        public void SetRigidbody(Rigidbody rb);

        /// <summary>
        /// Set the damageable of the tank.
        /// </summary>
        public void SetDamageable(IDamageableView damageable);

        /// <summary>
        /// Set the opposition layer mask of the tank.
        /// </summary>
        public void SetOppositionLayerMask(LayerMask layerMask);

        /// <summary>
        /// Initialize the brain
        /// It should be called after the brain is created or gotten from the pool
        /// </summary>
        public void Init();

        /// <summary>
        /// De-initialize the brain and return the
        /// view to the pool
        /// </summary>
        public void DeInit();

        /// <summary>
        /// This method helps in multiplayer mode.
        /// It deinitializes the specific initialized properties of the entity and 
        /// return the view to the pool
        /// </summary>
        public void DeInitNonServer();

        /// <summary>
        /// It is called when the entity is dead.
        /// It is separate from DeInit.
        /// </summary>
        public void ExecuteRagdollEffectEvent();
    }

}
