using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Entity
{
    public class EntityHealthController : MonoBehaviour, IEntityHealthController, IDamageable
    {
        public event Action<int, int> OnHealthUpdated;
        public event Action OnDamageTaken;

        private IEntityController m_Controller;
        private Collider m_DamageCollider;        // the collider that will be used to detect the damage
        public Transform Transform => transform;

        private int m_MaxHealth => m_Controller.MaxHealth;
        private int m_CurrentHealth;

        public void SetController(IEntityController controller) => m_Controller = controller;
        public void SetCollider(Collider collider) => m_DamageCollider = collider;

        public void ToggleCollider(bool value) => m_DamageCollider.enabled = value;

        public void TakeDamage(int damage)
        {
            AddHealth(-damage);
            OnDamageTaken?.Invoke();

            if (m_CurrentHealth == 0)
            {
                m_Controller.Die();
            }
        }

        public void Reset()
        {
            _ = HelperMethods.InvokeInNextFrame(() => AddHealth(m_MaxHealth));
        }

        private void AddHealth(int health)
        {
            m_CurrentHealth += health;
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
            OnHealthUpdated?.Invoke(m_CurrentHealth, m_MaxHealth);
        }
    }
}