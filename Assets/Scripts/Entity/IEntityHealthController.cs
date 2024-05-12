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
        /// This method is used to take damage.
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage);
    }
}