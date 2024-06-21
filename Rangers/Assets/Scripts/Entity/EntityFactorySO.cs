using BTG.Factory;


namespace BTG.Entity
{
    /// <summary>
    /// An abstract factory that creates the entities of the project
    /// Any entity factory must inherit from this class
    /// Entity types must implement the IEntityBrain interface
    /// </summary>
    public abstract class EntityFactorySO : FactorySO<IEntityBrain>
    {
        public abstract override IEntityBrain GetItem();

        public abstract IEntityBrain GetNonServerItem();
    }
}


