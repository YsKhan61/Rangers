using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.DIExample
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