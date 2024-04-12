using UnityEngine;


namespace BTG.Actions.UltimateAction
{
    public abstract class UltimateActionFactorySO : ScriptableObject
    {
        public abstract IUltimateAction CreateUltimateAction(IUltimateActor controller);
    }

}
