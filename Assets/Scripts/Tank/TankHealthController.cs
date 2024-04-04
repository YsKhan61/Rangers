using System.Threading.Tasks;
using UnityEngine;

namespace BTG.Tank
{
    public class TankHealthController
    {
        public event System.Action<int, int> OnTankHealthUpdated;        // int - CurrentHealth, int - MaxHealth

        private TankModel m_Model;
        private TankBrain m_MainController;

        public TankHealthController(TankModel model, TankBrain controller)
        {
            m_Model = model;
            m_MainController = controller;
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

            if (m_Model.CurrentHealth <= 0)
            {
                Debug.Log("Tank is dead: " + m_Model.Name);
                m_MainController.Die();
            }
        }
    }
}