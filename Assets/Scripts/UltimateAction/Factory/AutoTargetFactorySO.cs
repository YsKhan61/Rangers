using UnityEngine;


[CreateAssetMenu(fileName = "Auto Target Factory", menuName = "ScriptableObjects/UltimateActionFactory/AutoTargetFactorySO")]
public class AutoTargetFactorySO : UltimateActionFactorySO
{
    public override IUltimateAction CreateUltimateAction()
    {
        return new AutoTarget();
    }
}
