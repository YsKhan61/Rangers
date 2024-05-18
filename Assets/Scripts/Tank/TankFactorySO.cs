using BTG.Entity;
using UnityEngine;


namespace BTG.Tank
{
    /// <summary>
    /// This factory is responsible for creating the tanks.
    /// It can create all types of tanks according to the data given
    /// </summary>
    [CreateAssetMenu(fileName = "TankFactory", menuName = "ScriptableObjects/Factory/EntityFactory/TankFactorySO")]
    public class TankFactorySO : EntityFactorySO
    {
        [SerializeField]
        TankDataSO m_Data;

        TankPool m_Pool;
        public TankPool Pool => m_Pool ??= new(m_Data);

        public override IEntityBrain GetItem() => Pool.GetTank();
    }
}


