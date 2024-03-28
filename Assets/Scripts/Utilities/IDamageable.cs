using UnityEngine;

namespace BTG.Utilities
{
    public interface IDamageable
    {
        public Transform Transform { get; }
        public void TakeDamage(int damage);
    }
}