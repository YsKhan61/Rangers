using System.Collections.Generic;
using UnityEngine;

namespace BTG.Tank
{
    public class TankFactory
    {
        private TankDataContainerSO m_TankDataContainer;

        private List<TankPoolItem> m_Pools;

        public TankFactory(TankDataContainerSO container)
        {
            m_TankDataContainer = container;
            CreatePoolItems();
        }

        public bool TryGetRandomTank(out TankBrain controller)
        {
            int randomIndex = Random.Range(0, m_TankDataContainer.TankDataList.Length);
            return TryGetTank(m_TankDataContainer.TankDataList[randomIndex].ID , out controller);
        }

        public bool TryGetTank(int tankId, out TankBrain controller)
        {
            controller = null;

            if (!TryGetTankDataById(tankId, out TankDataSO tankDataToSpawn))            // m_TankID is for test purpose
                return false;

            controller = m_Pools.Find(pool => pool.Id == tankId).Pool.GetTank();
            if (controller == null)
            {
                Debug.LogError("TankMainController is null in TankFactory");
                return false;
            }

            return true;
        }

        private bool TryGetTankDataById(in int id, out TankDataSO tankData)
        {
            tankData = null;

            if (m_TankDataContainer == null || m_TankDataContainer.TankDataList.Length == 0)
            {
                Debug.LogError("TankDataList is not set or empty in PlayerTankSpawner");
                return false;
            }

            foreach (var tank in m_TankDataContainer.TankDataList)
            {
                if (tank.ID == id)
                {
                    tankData = tank;
                    return true;
                }
            }
            return false;
        }

        private void CreatePoolItems()
        {
            m_Pools = new List<TankPoolItem>();
            foreach (var tankData in m_TankDataContainer.TankDataList)
            {
                TankPool pool = new TankPool(tankData);
                TankPoolItem poolItem = new TankPoolItem
                {
                    Pool = pool,
                    Id = tankData.ID
                };
                m_Pools.Add(poolItem);
            }
        }

        internal class TankPoolItem
        {
            public TankPool Pool;
            public int Id;
        }
    }
}