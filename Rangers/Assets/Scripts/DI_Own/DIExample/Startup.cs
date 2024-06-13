using BTG.Utilities;
using UnityEngine;

namespace BTG.DI_Own.Example
{
    public class Startup : MonoBehaviour, IDependencyProviderForOthers, IDependencyInjector
    {
        [SerializeField]
        IntDataSO healthData;

        [Provide]
        public IntDataSO ProvideHealth() => healthData;

        /*PlayerService playerService;
        EnemyService enemyService;*/

        [Inject]
        GameManager gameManager;

        PlayerService playerService;

        [Inject]
        EnemyService enemyService;

        private void Awake()
        {
            /*enemyService = new EnemyService();
            DIManager.Instance.RegisterInstance(enemyService);*/
            // enemyService = (EnemyService)DIManager.Instance.RegisterType(typeof(EnemyService));
        }

        /*private void Awake()
        {
            playerService = new PlayerService();
            enemyService = new EnemyService();
            gameManager = new GameManager(playerService, enemyService, healthData);
        }*/

        private void Start()
        {
            // DIManager.Instance.Inject(enemyService);

            playerService = (PlayerService)DIManager.Instance.Resolve(typeof(PlayerService));

            gameManager.CheckAllDependencies();
            playerService.CheckAllDependencies();

            healthData.Value = 100;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // playerService.DeductHealth(10);
                enemyService.AttackPlayer();
            }
        }
    }
}