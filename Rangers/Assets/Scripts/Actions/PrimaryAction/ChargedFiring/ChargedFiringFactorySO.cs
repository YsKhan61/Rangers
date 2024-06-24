using BTG.Factory;
using UnityEngine;
using VContainer;

namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "Charged Firing Factory", menuName = "ScriptableObjects/Factory/PrimaryActionFactory/ChargedFiringFactorySO")]
    public class ChargedFiringFactorySO : PrimaryActionFactorySO
    {
        [Inject]
        private IObjectResolver m_Resolver;

        [SerializeField]
        ChargedFiringDataSO m_Data;

        ProjectilePool m_Pool;
        ProjectilePool Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    m_Pool = new ProjectilePool(m_Data);
                    m_Resolver.Inject(m_Pool);
                }
                return m_Pool;
            }
        }

        public override IPrimaryAction GetItem()
        {
            ChargedFiring cf = new (m_Data, Pool);
            m_Resolver.Inject(cf);
            return cf;
        }

        public override IPrimaryAction GetNetworkItem()
        {
            return GetItem();
        }
    }
}