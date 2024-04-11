using BTG.Entity;
using BTG.Tank;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService
    {
        [Inject]
        private EntityFactoryContainerSO m_EntityFactory;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

        /*[Inject]
        private TankFactory m_TankFactory;*/

        [Inject]
        private PlayerVirtualCamera m_PVCamera;


        private PlayerController m_PlayerController;

        private CancellationTokenSource m_CTS;

        public void Initialize()
        {
            CreatePlayerControllerAndInput();
            m_CTS = new CancellationTokenSource();
            
            m_PlayerStats.ResetStats();

            m_PlayerStats.TankIDSelected.OnValueChanged += Respawn;
        }

        ~PlayerService()
        {
            m_PlayerStats.TankIDSelected.OnValueChanged -= Respawn;

            m_CTS.Cancel();
            m_CTS.Dispose();
        }

        public void OnEntityInitialized(Sprite icon)
        {
            m_PlayerStats.PlayerIcon.Value = icon;
        }

        public void OnPlayerDeath()
        {
            m_PlayerStats.DeathCount.Value++;
        }

        private void CreatePlayerControllerAndInput()
        {
            m_PlayerController = new PlayerController(this, m_PlayerData);
            DIManager.Instance.Inject(m_PlayerController);
            PlayerInputs playerInput = new(m_PlayerController);
            playerInput.Initialize();
        }

        private void Respawn(int _)
        {
            bool tankFound = CreateAndSpawnPlayerTank(out TankBrain tank);
            if (!tankFound)
                return;

            ConfigureTankAndController(tank);

            ConfigureTankWithCamera(tank);
        }

        private bool CreateAndSpawnPlayerTank(out TankBrain tank)
        {
            /*if (!m_TankFactory.TryGetTank(m_PlayerStats.TankIDSelected.Value, out tank))
            {
                return false;
            }*/

            tank = null;

            bool factoryFound = m_EntityFactory.TryGetFactory(m_PlayerStats.TankIDSelected.Value, out EntityFactorySO factory);
            if (!factoryFound)
            {
                Debug.Log("Factory not found!");
                return false;
            }

            tank = factory.GetEntity() as TankBrain;

            return true;
        }

        private void ConfigureTankAndController(TankBrain tank)
        {
            m_PlayerController.Transform.position = Vector3.zero;
            m_PlayerController.Transform.rotation = Quaternion.identity;
            m_PlayerController.SetEntity(tank);
        }

        private void ConfigureTankWithCamera(TankBrain tank)
        {
            m_PVCamera.Initialize(tank.CameraTarget);
        }
    }
}

