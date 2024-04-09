using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.Services
{
    public class DependencyProviderService : MonoBehaviour, IDependencyProvider
    {
        [SerializeField]
        PlayerStatsSO playerStats;

        [SerializeField]
        TankDataContainerSO tankDataList;

        [Provide]
        public PlayerStatsSO ProvidePlayerStats() => playerStats;

        [Provide]
        public IntDataSO ProvideEliminatedEnemiesCount() => playerStats.EliminatedEnemiesCount;

        [Provide]
        public TankDataContainerSO ProvideTankDataList() => tankDataList;
    }
}
