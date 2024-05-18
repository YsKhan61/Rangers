using BTG.Factory;
using UnityEngine;

namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "Tesla Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/TeslaFiringFactorySO")]
    public class TeslaFiringFactorySO : FactorySO<IPrimaryAction>
    {
        [SerializeField]
        TeslaFiringDataSO m_Data;

        TeslaBallPool m_Pool;
        TeslaBallPool Pool => m_Pool ??= new (m_Data);

        public override IPrimaryAction GetItem() => new TeslaFiring(m_Data, Pool);
    }
}

