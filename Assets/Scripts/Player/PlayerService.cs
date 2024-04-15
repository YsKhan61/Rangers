using BTG.Entity;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService : ISelfDependencyProvider, IDependencyInjectable
    {
        [Inject]
        private EntityFactoryContainerSO m_EntityFactory;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

        [Inject]
        private PlayerVirtualCamera m_PVCamera;


        private PlayerTankController m_PlayerController;

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
            m_PlayerController = new PlayerTankController(this, m_PlayerData);
            DIManager.Instance.Inject(m_PlayerController);
            PlayerInputs playerInput = new(m_PlayerController);
            playerInput.Initialize();
        }

        private void Respawn(int _)
        {
            bool entityFound = CreateAndSpawnPlayerEntity(out IEntityBrain entity);
            if (!entityFound)
                return;

            ConfigureTankAndController(entity);
        }

        private bool CreateAndSpawnPlayerEntity(out IEntityBrain entity)
        {
            entity = null;

            bool factoryFound = m_EntityFactory.TryGetFactory(m_PlayerStats.TankIDSelected.Value, out EntityFactorySO factory);
            if (!factoryFound)
            {
                Debug.Log("Factory not found!");
                return false;
            }

            entity = factory.GetEntity();

            return true;
        }

        private void ConfigureTankAndController(IEntityBrain entity)
        {
            m_PlayerController.Transform.position = Vector3.zero;
            m_PlayerController.Transform.rotation = Quaternion.identity;
            m_PlayerController.ConfigureWithEntity(entity);

            m_PVCamera.Initialize(m_PlayerController.CameraTarget);
        }
    }
}

