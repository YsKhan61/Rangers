using UnityEngine;


[CreateAssetMenu(fileName = "Self Shield Factory", menuName = "ScriptableObjects/UltimateActionFactory/SelfShieldFactorySO")]
public class SelfShieldFactorySO : UltimateActionFactorySO
{
    public override IUltimateAction CreateUltimateAction()
    {
        return new SelfShield();
    }
}
