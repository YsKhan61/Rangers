using BTG.Factory;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "Charged Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/ChargedFiringFactorySO")]
    public class ChargedFiringFactorySO : FactorySO<IPrimaryAction>
    {
        [SerializeField]
        ChargedFiringDataSO m_Data;

        ProjectilePool m_Pool;
        ProjectilePool Pool => m_Pool ??= new (m_Data);

        public override IPrimaryAction GetItem() => new ChargedFiring(m_Data, Pool);
    }
}