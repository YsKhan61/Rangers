using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public abstract class PrimaryActionFactorySO : ScriptableObject
    {
        public abstract IPrimaryAction CreatePrimaryAction(IPrimaryActor actor);
    }
}