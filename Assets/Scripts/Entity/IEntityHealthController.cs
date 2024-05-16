namespace BTG.Entity
{
    /// <summary>
    /// An interface for the health controller of an entity.
    /// Any entity that has health and can take damage should implement this interface.
    /// </summary>
    public interface IEntityHealthController
    {
        public event System.Action<int, int> OnHealthUpdated;        // int - CurrentHealth, int - MaxHealth
        public event System.Action OnDamageTaken;

        /// <summary>
        /// Get or set the enable status of the entity.
        /// </summary>
        public bool IsEnabled { get; set; }


        /// <summary>
        /// Set the controller of the entity.
        /// Can be player or enemy.
        /// </summary>
        public void SetController(IEntityController controller);


        /// <summary>
        /// This method is used to take damage.
        /// </summary>
        public void Damage(int damage);

        /// <summary>
        /// Set the max health of the entity.
        /// </summary>
        public void SetMaxHealth();
    }
}