using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public interface ITeslaBallView : IFiringView
    {
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public SphereCollider Collider { get; }
        public void SetOwner(Transform owner);
        public void SetTeslaFiringData(TeslaFiringDataSO data);
        public void SetDamage(int damage);
        public void AddImpulseForce(float force);
        public void Show();
        public void Hide();
        public void ReturnToPool();
    }

}

