using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    [RequireComponent(typeof(Collider))]
    public class TankDamageableView : MonoBehaviour, IDamageable
    {
        [SerializeField] private Collider m_DamageableCollider;

        private TankBrain m_Brain;

        public Transform Transform => m_Brain.Transform;

        public void SetBrain(TankBrain tankBrain)
        {
            m_Brain = tankBrain;
        }

        public void TakeDamage(int damage) => m_Brain.TakeDamage(damage);

        public void ToogleDamageableCollider(bool activate)
            => m_DamageableCollider.enabled = activate;
    }
}