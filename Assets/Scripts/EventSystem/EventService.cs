namespace BTG.EventSystem
{
    public class EventService
    {
        private static EventService m_Instance;
        public static EventService Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new EventService();
                }
                return m_Instance;
            }
        }

        public EventController<bool> OnTankDead { get; private set; }

        public EventService()
        {
            OnTankDead = new EventController<bool>();
        }
    }
}
