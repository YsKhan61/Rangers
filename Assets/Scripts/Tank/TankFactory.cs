using UnityEngine;

namespace BTG.Tank
{
    public class TankFactory
    {
        private TankDataContainerSO m_TankDataList;

        public TankFactory(TankDataContainerSO tankDataList)
        {
            m_TankDataList = tankDataList;
        }

        public bool TryCreatePlayerTank(in int tankId, out TankController controller)
        {
            controller = null;

            if (!TryGetTankById(tankId, out TankDataSO tankDataToSpawn))            // m_TankID is for test purpose
                return false;

            controller = new TankController(tankDataToSpawn);
            return true;
        }

        private bool TryGetTankById(in int id, out TankDataSO tankData)
        {
            tankData = null;

            if (m_TankDataList == null || m_TankDataList.TankDataList.Length == 0)
            {
                Debug.LogError("TankDataList is not set or empty in PlayerTankSpawner");
                return false;
            }

            foreach (var tank in m_TankDataList.TankDataList)
            {
                if (tank.ID == id)
                {
                    tankData = tank;
                    return true;
                }
            }
            return false;
        }
    }
}