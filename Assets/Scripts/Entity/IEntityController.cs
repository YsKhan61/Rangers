using BTG.Utilities;

namespace BTG.Entity
{
    /// <summary>
    /// An interface for the controller of an entity.
    /// It can be player or enemy
    /// </summary>
    public interface IEntityController
    {
        /// <summary>
        /// Get the max health of the entity
        /// </summary>
        public int MaxHealth { get; }

        /// <summary>
        /// This method is used to inform that the entity has died.
        /// </summary>
        public void EntityDied();
    }
}