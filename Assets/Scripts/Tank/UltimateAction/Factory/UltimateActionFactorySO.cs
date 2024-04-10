using BTG.Entity;
using UnityEngine;


namespace BTG.Tank.UltimateAction
{
    public abstract class UltimateActionFactorySO : ScriptableObject
    {
        public abstract IEntityUltimateAbility CreateUltimateAction(TankUltimateController controller);
    }

}
