using BTG.Utilities;
using UnityEngine;

namespace BTG.DI_Own.Example
{
    public class  PlayerService : ISelfDependencyRegister, IDependencyInjector
    {
        [Inject]
        GameManager gameManager;

        [Inject]
        IntDataSO healthData;

        public void CheckAllDependencies()
        {
            Debug.Log("Checking all dependencies in PlayerService");

            if (gameManager == null)
            {
                Debug.LogError("GameManager is null");
            }
        }

        public void DeductHealth(int value)
        {
            healthData.Value -= value;
        }
    }
}