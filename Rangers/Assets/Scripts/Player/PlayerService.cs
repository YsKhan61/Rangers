using BTG.Entity;
using BTG.Events;
using BTG.EventSystem;
using BTG.Utilities;
using BTG.Utilities.EventBus;
using System.Threading;
using UnityEngine;
using VContainer;


namespace BTG.Player
{
    public class PlayerService
    {
        private const int RESPAWN_DELAY = 2;

        [Inject]
        private EntityFactoryContainerSO m_EntityFactoryContainer;

        [Inject]
        private PlayerDataSO m_PlayerData;

        [Inject]
        private PlayerStatsSO m_PlayerStats;

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

            CreatePlayerController();
            m_PlayerStats.ResetStats();
            m_PlayerStats.EntityTagSelected.OnValueChanged += OnPlayerTankIDSelected;
        }

        ~PlayerService()
        {
            m_PlayerStats.EntityTagSelected.OnValueChanged -= OnPlayerTankIDSelected;
            HelperMethods.CancelAndDisposeCancellationTokenSource(m_CTS);
        }

        public void SetPlayerVirtualCamera(PlayerVirtualCamera pvc)
        {
            m_PVCamera = pvc;
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

        private void CreatePlayerController()
        {
            m_Controller = new PlayerTankController.Builder()
                .CreateModel(m_PlayerData)
                .CreateView(m_PlayerData.Prefab)
                .WithPlayerService(this)
                .CreatePlayerInput()
                .Build();
        }

        private void OnPlayerTankIDSelected()
        {
            EventBus<CameraShakeEvent>.Invoke(new CameraShakeEvent { ShakeAmount = 0f, ShakeDuration = 0f });

            /// If there is a player entity already, deinit it.
            m_Controller.DeInit();

            _ = HelperMethods.InvokeInNextFrame(SpawnPlayerEntity);
        }

        private void SpawnPlayerEntity()
        {
            bool entityFound = TryGetEntity(out IEntityBrain entity);
            if (!entityFound)
                return;

            m_Controller.SetEntityBrain(entity);
            m_Controller.Init();
            // Spawn at the origin
            m_Controller.SetPose(new Pose(Vector3.zero, Quaternion.identity));
            m_PVCamera.SetFollowTarget(m_Controller.CameraTarget);
        }

        private bool TryGetEntity(out IEntityBrain entity)
        {
            entity = m_EntityFactoryContainer.GetFactory(m_PlayerStats.EntityTagSelected.Value).GetItem();
            return entity != null;
        }
    }
}

