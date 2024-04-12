using BTG.Actions.UltimateAction;
using System;
using UnityEngine;


namespace BTG.Entity
{
    public interface IEntityTankBrain : IEntityBrain, IUltimateActor
    {
        public event Action<float, float> OnPlayerCamShake;
        public event Action<Sprite> OnEntityInitialized;
        public event Action OnAfterDeath;

        public IEntityTankModel Model { get; }
        public Transform CameraTarget { get; }
        public IUltimateAction UltimateAction { get; }
        public IEntityFiringController FiringController { get; }
        public IEntityHealthController HealthController { get; }

        public void SetLayers(int selfLayer, int oppositionLayer);
        public void SetParentOfView(Transform parent, Vector3 position, Quaternion rotation);
        public void SetRigidbody(Rigidbody rb);
        public void Init();
        public void StartFire();
        public void StopFire();
    }
}
