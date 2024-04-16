using BTG.Utilities;
using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.DIExample
{
    public class GameManager : ISelfDependencyRegister, IDependencyInjector
    {
        [Inject]
        private PlayerService playerService;


        public void CheckAllDependencies()
        {
            Debug.Log("Checking all dependencies in GameManager");

            if (playerService == null)
            {
                Debug.LogError("PlayerService is null");
            }

        }
    }
}