using BTG.Entity;
using BTG.Utilities;
using UnityEngine;

namespace BTG.Tank
{
    [CreateAssetMenu(fileName = "TankFactory", menuName = "ScriptableObjects/Factory/EntityFactory/TankFactorySO")]
    public class TankFactorySO : EntityFactorySO
    {
        [SerializeField]
        TagSO m_EntityTag;

        [SerializeField]
        TankDataSO m_Data;

        TankPool m_Pool;
        public TankPool Pool => m_Pool ??= new(m_Data);

        public override IEntityBrain GetEntity() => Pool.GetTank();

        public override void ReturnEntity(IEntityBrain tank) => Pool.ReturnTank(tank as TankBrain);
    }
}


