using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public interface IProjectileView : IFiringView
    { 
        public Transform Transform { get; }
        public Rigidbody Rigidbody { get; }
        public void SetOwner(Transform owner);
        public void Show();
        public void Hide();
        public void ReturnToPool();
    }
}

