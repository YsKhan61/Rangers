using System;
using UnityEngine;

namespace BTG.Entity
{
    public interface IEntityBrain
    {
        public event Action<Sprite> OnEntityInitialized;
        public event Action OnAfterDeath;

        public IEntityModel Model { get; }

        public Transform Transform { get; }
        public Transform CameraTarget { get; }

        public IEntityUltimateController UltimateController { get; }
        public IEntityUltimateAbility UltimateAction { get; }
        public IEntityFiringController FiringController { get; }
        public IEntityHealthController HealthController { get; }

        public void SetLayers(int selfLayer, int oppositionLayer);

        public void SetParentOfView(Transform parent, Vector3 position, Quaternion rotation);

        public void SetRigidbody(Rigidbody rb);

        public void Init();

        public void StartFire();

        public void StopFire();

        public void TryExecuteUltimate();
    }
}
