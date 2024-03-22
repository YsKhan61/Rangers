using UnityEngine;


[CreateAssetMenu(fileName = "Invisibility Factory", menuName = "ScriptableObjects/UltimateActionFactory/InvisibilityFactorySO")]
public class InvisibilityFactorySO : UltimateActionFactorySO
{
    public override IUltimateAction CreateUltimateAction()
    {
        return new Invisibility();
    }
}
