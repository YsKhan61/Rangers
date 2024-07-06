using BTG.Factory;


namespace BTG.Actions.PrimaryAction
{
    /// <summary>
    /// This abstract class is used to create asset of the primary action factory,
    /// as the factory is a generic class and can't be created as an asset.
    /// </summary>
    public abstract class PrimaryActionFactorySO : FactorySO<IPrimaryAction>
    {
        /// <summary>
        /// Multiplayer - This method is used to get the network item from the factory
        /// </summary>
        public abstract IPrimaryAction GetNetworkItem();
    }
}