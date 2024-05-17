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

        public EventController<bool> OnBeforeAnyTankDead { get; private set; }

        /// <summary>
        /// This event is called when a new player is needed to be spawned.
        /// It will show the HeroSelectionUI panel for the player to select the tank.
        /// </summary>
        public EventController OnShowHeroSelectionUI { get; private set; }

        public EventService()
        {
            OnBeforeAnyTankDead = new EventController<bool>();
            OnShowHeroSelectionUI = new EventController();
        }
    }
}
