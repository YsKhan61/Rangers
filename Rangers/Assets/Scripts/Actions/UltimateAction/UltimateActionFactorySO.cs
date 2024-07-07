using BTG.Factory;


namespace BTG.Actions.UltimateAction
{
    /// <summary>
    /// An abstract factory scriptable object for the ultimate action.
    /// Any concrete ultimate action factory scriptable object should inherit from this class
    /// </summary>
    public abstract class  UltimateActionFactorySO : FactorySO<IUltimateAction>
    {
        /// <summary>
        /// Multiplayer - Get the item that will be used in the multiplayer scene.
        /// </summary>
        /// <returns></returns>
        public abstract IUltimateAction GetNetworkItem();
    }
}

