using UnityEngine;


namespace BTG.DI_Own.Example
{
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