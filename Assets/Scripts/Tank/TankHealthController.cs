using System.Threading.Tasks;
using UnityEngine;

namespace BTG.Tank
{
    public class TankHealthController
    {
        public event System.Action<int, int> OnTankHealthUpdated;        // int - CurrentHealth, int - MaxHealth

        private TankModel m_Model;
        private TankBrain m_Brain;

        public TankHealthController(TankModel model, TankBrain brain)
        {
            m_Model = model;
            m_Brain = brain;
        }

        ~TankHealthController()
        {
            
        }

        public async void Init()
        {
            await Task.Yield();             // wait a frame to make sure UI is ready
            AddHealth(m_Model.MaxHealth);
        }

        public void AddHealth(int health)
        {
            m_Model.AddHealthData(health);
            OnTankHealthUpdated?.Invoke(m_Model.CurrentHealth, m_Model.MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            AddHealth(-damage);

            if (m_Model.CurrentHealth > 0) return;

            m_Brain.Die();
        }
    }
}