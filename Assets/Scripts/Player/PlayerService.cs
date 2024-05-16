using BTG.Entity;
using BTG.EventSystem;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService : ISelfDependencyRegister, IDependencyInjector
    {
        private const int RESPAWN_DELAY = 2;

        [Inject]
        private EntityFactoryContainerSO m_EntityFactory;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

        [Inject]
        private PlayerVirtualCamera m_PVCamera;


        private PlayerTankController m_Controller;

        private CancellationTokenSource m_CTS;

        /// <summary>
        /// Create and spawn the player entity.
        /// Create the player controller and input.
        /// </summary>
        public void Initialize()
        {
            m_CTS = new CancellationTokenSource();

            CreatePlayerControllerAndInput();
            m_PlayerStats.ResetStats();
            m_PlayerStats.TankIDSelected.OnValueChanged += OnPlayerTankIDSelected;
        }

        ~PlayerService()
        {
            m_PlayerStats.TankIDSelected.OnValueChanged -= OnPlayerTankIDSelected;
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);
        }

        public void OnEntityInitialized(Sprite icon)
        {
            m_PlayerStats.PlayerIcon.Value = icon;
        }

        public void OnPlayerDeath()
        {
            m_PlayerStats.DeathCount.Value++;

            Respawn();
        }

        private void Respawn()
        {
            _ = HelperMethods.InvokeAfterAsync(
                RESPAWN_DELAY, 
                () => EventService.Instance.OnShowHeroSelectionUI.InvokeEvent(), 
                m_CTS.Token);
        }

        private void CreatePlayerControllerAndInput()
        {
            m_Controller = new PlayerTankController(m_PlayerData);
            DIManager.Instance.Inject(m_Controller);
            PlayerInputs playerInput = new(m_Controller);
            playerInput.Initialize();
        }

        private void OnPlayerTankIDSelected()
        {
            /// If there is a player entity already, deinit it.
            m_Controller.DeInit();

            _ = HelperMethods.InvokeInNextFrame(SpawnPlayerEntity);
        }

        private void SpawnPlayerEntity()
        {
            bool entityFound = CreateAndSpawnPlayerEntity(out IEntityBrain entity);
            if (!entityFound)
                return;

            m_Controller.SetEntityBrain(entity);
            m_Controller.Init();
            // Spawn at the origin
            m_Controller.SetPose(new Pose(Vector3.zero, Quaternion.identity));

            m_PVCamera.Initialize(m_Controller.CameraTarget);
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
    }
}

