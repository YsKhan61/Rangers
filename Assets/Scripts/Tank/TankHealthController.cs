using UnityEngine;

namespace BTG.Tank
{
    public class TankHealthController
    {
        private TankModel m_Model;
        private TankBrain m_MainController;

        public TankHealthController(TankModel model, TankBrain controller)
        {
            m_Model = model;
            m_MainController = controller;
            model.AddHealthData(model.TankData.MaxHealth);
        }

        ~TankHealthController()
        {
            
        }

        public void TakeDamage(int damage)
        {
            m_Model.AddHealthData(-damage);
            Debug.Log("Tank: " + m_Model.Name + " took damage: " + damage + " Current Health: " + m_Model.CurrentHealth);

            if (m_Model.CurrentHealth <= 0)
            {
                Debug.Log("Tank is dead: " + m_Model.Name);
                m_MainController.Die();
            }
        }
    }
}