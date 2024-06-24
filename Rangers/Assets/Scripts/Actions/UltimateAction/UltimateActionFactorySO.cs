using BTG.Factory;
using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public abstract class  UltimateActionFactorySO : FactorySO<IUltimateAction>
    {
        public abstract IUltimateAction GetNetworkItem();
    }
}

