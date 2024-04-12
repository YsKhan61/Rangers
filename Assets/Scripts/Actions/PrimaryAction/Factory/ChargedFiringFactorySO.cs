using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "Charged Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/ChargedFiringFactorySO")]
    public class ChargedFiringFactorySO : PrimaryActionFactorySO
    {
        [SerializeField]
        ChargedFiringDataSO m_Data;

        ProjectilePool s_Pool;
        ProjectilePool Pool => s_Pool ??= new ProjectilePool(m_Data);


        public override IPrimaryAction CreatePrimaryAction(IPrimaryActor actor)
            => new ChargedFiring(m_Data, actor, Pool);
    }
}