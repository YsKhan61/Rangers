using UnityEngine;


[CreateAssetMenu(fileName = "AirStrike Factory", menuName = "ScriptableObjects/UltimateActionFactory/AirStrikeFactorySO")]
public class AirStrikeFactorySO : UltimateActionFactorySO
{
    public override IUltimateAction CreateUltimateAction()
    {
        return new AirStrike();
    }
}
