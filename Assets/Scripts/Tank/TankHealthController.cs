using BTG.Entity;
using System.Threading.Tasks;


namespace BTG.Tank
{
    public class TankHealthController : IEntityHealthController
    {
        public event System.Action<int, int> OnHealthUpdated;        // int - CurrentHealth, int - MaxHealth
        public event System.Action OnDamageTaken;

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

        public async void Reset()
        {
            await Task.Yield();             // wait a frame to make sure UI is ready
            AddHealth(m_Model.MaxHealth);
        }

        public void AddHealth(int health)
        {
            m_Model.AddHealth(health);
            OnHealthUpdated?.Invoke(m_Model.CurrentHealth, m_Model.MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            AddHealth(-damage);
            OnDamageTaken?.Invoke();

            if (m_Model.CurrentHealth < 0)
            {
                m_Brain.Die();
            }
        }
    }
}