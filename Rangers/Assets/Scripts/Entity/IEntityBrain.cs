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
        /// Set the controller of the Entity
        /// </summary>
        public void SetController(IEntityController controller);

        /// <summary>
        /// Set the parent of the tank view.
        /// It is used to set the parent of the tank view to the tank container or Player/Enemy View
        /// </summary>
        public void SetParentOfView(Transform parent, Vector3 position, Quaternion rotation);

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
        /// This method helps in multiplayer mode.
        /// It initializes the specific properties of the entity such as audiosource, animator etc.
        /// </summary>
        public void InitNonServer();

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

        /// <summary>
        /// This method helps in setting the visibility of the entity.
        /// </summary>
        public void InitializeChargingAndShootingClips(AudioClip chargingClip, AudioClip shootClip);

        /// <summary>
        /// This method helps in playing the charging clip of the primary action of the entity.
        /// </summary>
        public void PlayChargingClip();

        /// <summary>
        /// This method helps in updating the pitch of the charging clip of the primary action of the entity.
        /// </summary>
        public void UpdateChargingClipPitch(float amount);

        /// <summary>
        /// This method helps in playing the shot fired clip of the primary action of the entity.
        /// </summary>
        /// <param name="clip"></param>
        public void PlayShotFiredClip();
    }

}
