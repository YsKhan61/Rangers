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
        /// Set the controller of the entity.
        /// Can be player or enemy.
        /// </summary>
        public void SetController(IEntityController controller);

        /// <summary>
        /// Set the collider that will be used to detect the damage.
        /// </summary>
        public void SetCollider(UnityEngine.Collider collider);

        /// <summary>
        /// Activate or deactivate the collider to decide whether the entity can take damage or not.
        /// </summary>
        public void ToggleCollider(bool value);

        /// <summary>
        /// This method is used to take damage.
        /// </summary>
        public void TakeDamage(int damage);

        /// <summary>
        /// Reset the health of the entity.
        /// </summary>
        public void Reset();
    }
}