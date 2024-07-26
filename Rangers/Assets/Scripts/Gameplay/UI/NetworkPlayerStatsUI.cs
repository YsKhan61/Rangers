using UnityEngine.InputSystem;
using UnityEngine;
using BTG.Gameplay.GameplayObjects;
using System.Collections.Generic;


namespace BTG.Gameplay.UI
{
    public class NetworkPlayerStatsUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel that shows the player stats")]
        private GameObject m_PlayerStatsPanel;

        [SerializeField]
        InputActionReference m_InputActionReference;

        [SerializeField]
        private PersistentPlayersRuntimeCollectionSO _persistentPlayerRuntimeData;

        [SerializeField, Tooltip("The parent under which the row UIs will be placed")]
        private Transform m_StatsContainer;

        [SerializeField]
        private PlayerStatsRowUI playerStatsRowUIPrefab;

        private readonly List<PlayerStatsRowUI> _playerStatsRowUIList = new();

        private void OnEnable()
        {
            m_InputActionReference.action.Enable();
            m_InputActionReference.action.performed += OnInputActionPerformed;
        }

        private void OnDisable()
        {
            m_InputActionReference.action.performed -= OnInputActionPerformed;
            m_InputActionReference.action.Disable();
        }

        private void Start()
        {
            HidePanel();
        }

        /// <summary>
        /// Adds a new row to the player stats panel for the given client ID.
        /// </summary>
        /// <param name="clientId">the client ID of the player to add</param>
        public void AddPlayerStatRow(ulong clientId)
        {
            if (!_persistentPlayerRuntimeData.TryGetPlayer(clientId, out var persistentPlayer))
            {
                return;
            }

            var playerStatsRowUI = Instantiate(playerStatsRowUIPrefab, m_StatsContainer);
            _playerStatsRowUIList.Add(playerStatsRowUI);
            playerStatsRowUI.ClientId = clientId;
            var playerName = persistentPlayer.NetworkNameState.Name.Value;
            playerStatsRowUI.NameText.text = playerName;

            RegisterToPersistentPlayer(persistentPlayer);
        }

        /// <summary>
        /// Removes the row for the given client ID from the player stats panel.
        /// </summary>
        /// <param name="clientId">the client ID of the player to remove</param>
        public void RemovePlayerStatRow(ulong clientId)
        {
            foreach (var playerStatsRowUI in _playerStatsRowUIList)
            {
                if (playerStatsRowUI.ClientId == clientId)
                {
                    if (!_persistentPlayerRuntimeData.TryGetPlayer(clientId, out var persistentPlayer))
                    {
                        return;
                    }
                    UnregisterFromPersistentPlayer(persistentPlayer);

                    _playerStatsRowUIList.Remove(playerStatsRowUI);
                    Destroy(playerStatsRowUI.gameObject);
                    break;
                }
            }
        }

        private void RegisterToPersistentPlayer(PersistentPlayer persistentPlayer)
        {
            persistentPlayer.NetworkStatsState.OnKillsChanged += NetworkStatsState_OnKillsChanged;
            persistentPlayer.NetworkStatsState.OnDeathsChanged += NetworkStatsState_OnDeathsChanged;
        }

        private void UnregisterFromPersistentPlayer(PersistentPlayer persistentPlayer)
        {
            persistentPlayer.NetworkStatsState.OnKillsChanged -= NetworkStatsState_OnKillsChanged;
            persistentPlayer.NetworkStatsState.OnDeathsChanged -= NetworkStatsState_OnDeathsChanged;
        }

        private void NetworkStatsState_OnKillsChanged(ulong clientId, int value)
        {
            if (!TryGetPlayerStatsRowUI(clientId, out var playerStatsRowUI))
            {
                return;
            }
            playerStatsRowUI.KillText.text = value.ToString();
        }

        private void NetworkStatsState_OnDeathsChanged(ulong clientId, int value)
        {
            if (!TryGetPlayerStatsRowUI(clientId, out var playerStatsRowUI))
            {
                return;
            }
            playerStatsRowUI.DeathText.text = value.ToString();
        }

        private void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            if (m_PlayerStatsPanel.activeSelf)
                HidePanel();
            else
                ShowPanel();
        }

        private void ShowPanel() => m_PlayerStatsPanel.SetActive(true);
        
        private void HidePanel() => m_PlayerStatsPanel.SetActive(false);

        private bool TryGetPlayerStatsRowUI(ulong clientId, out PlayerStatsRowUI playerStatsRowUI)
        {
            foreach (var rowUI in _playerStatsRowUIList)
            {
                if (rowUI.ClientId == clientId)
                {
                    playerStatsRowUI = rowUI;
                    return true;
                }
            }

            Debug.LogError($"Could not find player stats row for clientId: {clientId}");

            playerStatsRowUI = null;
            return false;
        }
    }

}