using BTG.Events;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System;
using UnityEngine;


namespace BTG.Entity
{
    public class EntityHealthController : MonoBehaviour, IDamageableView
    {
        public event Action<int, int> OnHealthUpdated;
        public event Action OnDamageTaken;

        private IEntityController m_Controller;
        public Transform Transform => transform;

        private int m_MaxHealth => m_Controller.MaxHealth;
        private int m_CurrentHealth;

        /// <summary>
        /// Get or set the enable status of the entity.
        /// </summary>
        public bool IsEnabled { get; set; }

        public Transform Owner { get; private set; }

        public bool IsPlayer {get; private set; }

        public bool IsVisible { get; private set; }

        public bool CanTakeDamage { get; private set; }

        /// <summary>
        /// Set the controller of the entity.
        /// Can be player or enemy.
        /// </summary>
        public void SetController(IEntityController controller) => m_Controller = controller;

        /// <summary>
        /// Set the owner of the entity.
        /// Also set if the owner is a player or not.
        /// </summary>
        public void SetOwner(Transform owner, bool isPlayer)
        {
            IsPlayer = isPlayer;
            Owner = owner;
        }

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
                m_Controller.OnEntityDied();
                IsEnabled = false;
            }
        }

        public void Damage(ulong actorOwnerClientId, int damage)
        {
            if (!IsEnabled)
            {
                return;
            }

            AddHealth(-damage);
            OnDamageTaken?.Invoke();

            if (m_CurrentHealth == 0)
            {
                m_Controller.OnEntityDied();
                IsEnabled = false;
                
                EventBus<KillDeathEventData>.Invoke(new KillDeathEventData(actorOwnerClientId, (m_Controller as INetworkEntityController).OwnerClientId));
            }
        }

        /// <summary>
        /// Set the max health of the entity.
        /// </summary>
        public void SetMaxHealth()
        {
            if (!IsEnabled)
            {
                return;
            }

            AddHealth(m_MaxHealth);
        }

        public void SetVisible(bool isVisible) => IsVisible = isVisible;

        private void AddHealth(int health)
        {
            m_CurrentHealth += health;
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth, 0, m_MaxHealth);
            OnHealthUpdated?.Invoke(m_CurrentHealth, m_MaxHealth);
        }
    }
}