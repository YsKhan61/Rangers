using UnityEngine;

namespace BTG.Actions.PrimaryAction
{

    [CreateAssetMenu(fileName = "Tesla Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/TeslaFiringFactorySO")]
    public class TeslaFiringFactorySO : PrimaryActionFactorySO
    {
        [SerializeField]
        TeslaFiringDataSO m_Data;

        TeslaBallPool m_Pool;
        TeslaBallPool Pool => m_Pool ??= new (m_Data);

        public override IPrimaryAction CreatePrimaryAction(IPrimaryActor actor)
            => new TeslaFiring(m_Data, actor, Pool);
    }
}

