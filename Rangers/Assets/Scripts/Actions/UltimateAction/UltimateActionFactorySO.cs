using BTG.Factory;


namespace BTG.Actions.UltimateAction
{
    public abstract class  UltimateActionFactorySO : FactorySO<IUltimateAction>
    {
        public abstract IUltimateAction GetNetworkItem();
    }
}

