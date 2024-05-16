using BTG.Utilities;
using System;
using UnityEngine;

namespace BTG.Entity
{
    public class EntityHealthController : MonoBehaviour, IEntityHealthController, IDamageableView
    {
        public event Action<int, int> OnHealthUpdated;
        public event Action OnDamageTaken;

        private IEntityController m_Controller;
        public Transform Transform => transform;

        private int m_MaxHealth => m_Controller.MaxHealth;
        private int m_CurrentHealth;
        public bool IsEnabled { get; set; }

        public void SetController(IEntityController controller) => m_Controller = controller;

        public void Damage(int damage)
        {
            if (!IsEnabled)
            {
                return;
            }

            AddHealth(-damage);
            OnDamageTaken?.Invoke();

            if (m_CurrentHealth == 0)
            {
                m_Controller.EntityDied();
                IsEnabled = false;
            }
        }

        public void SetMaxHealth()
        {
            if (!IsEnabled)
            {
                return;
            }

            AddHealth(m_MaxHealth);
        }

        private void AddHealth(int health)
        {
            m_CurrentHealth += health;
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
            OnHealthUpdated?.Invoke(m_CurrentHealth, m_MaxHealth);
        }
    }
}