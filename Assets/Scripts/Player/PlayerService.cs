using BTG.Entity;
using BTG.Utilities;
using BTG.Utilities.DI;
using System.Threading;
using System.Xml;
using UnityEngine;


namespace BTG.Player
{
    public class PlayerService : ISelfDependencyRegister, IDependencyInjector
    {
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

        public void Initialize()
        {
            CreatePlayerControllerAndInput();
            m_CTS = new CancellationTokenSource();
            
            m_PlayerStats.ResetStats();

            m_PlayerStats.TankIDSelected.OnValueChanged += OnPlayerTankIDSelected;
        }

        ~PlayerService()
        {
            m_PlayerStats.TankIDSelected.OnValueChanged -= OnPlayerTankIDSelected;

            HelperMethods.DisposeCancellationTokenSource(m_CTS);
        }

        public void OnEntityInitialized(Sprite icon)
        {
            m_PlayerStats.PlayerIcon.Value = icon;
        }

        public void OnPlayerDeath()
        {
            m_Controller = null;
            m_PlayerStats.DeathCount.Value++;
        }

        private void CreatePlayerControllerAndInput()
        {
            m_Controller = new PlayerTankController(this, m_PlayerData);
            DIManager.Instance.Inject(m_Controller);
            PlayerInputs playerInput = new(m_Controller);
            playerInput.Initialize();
        }

        private void OnPlayerTankIDSelected()
        {
            if (m_Controller != null)
            {
                /// The current player entity is not null, so we need to deinitialize it.
                m_Controller.DeInit();
            }

            _ = HelperMethods.InvokeInNextFrame(Respawn);
        }

        private void Respawn()
        {
            bool entityFound = CreateAndSpawnPlayerEntity(out IEntityBrain entity);
            if (!entityFound)
                return;

            m_Controller.SetEntityBrain(entity);
            m_Controller.Init();
            entity.Init();
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

