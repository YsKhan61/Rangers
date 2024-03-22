using UnityEngine;

public abstract class UltimateActionFactorySO : ScriptableObject
{
    public abstract IUltimateAction CreateUltimateAction();
}
