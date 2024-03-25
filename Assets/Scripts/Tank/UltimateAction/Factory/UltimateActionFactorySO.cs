using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateActionFactorySO : ScriptableObject
    {
        public abstract IUltimateAction CreateUltimateAction(TankUltimateController controller);
    }

}
