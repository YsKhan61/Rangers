using BTG.Utilities.DI;
using UnityEngine;

namespace BTG.DIExample
{
    [Register]
    [Injector]
    public class  EnemyService
    {
        [Inject]
        PlayerService playerService;

        public void CheckAllDependencies()
        {
            Debug.Log("Checking all dependencies in EnemyService");

            if (playerService == null)
            {
                Debug.LogError("PlayerService is null");
            }
        }

        public void AttackPlayer()
        {
            playerService.DeductHealth(10);
        }
    }
}