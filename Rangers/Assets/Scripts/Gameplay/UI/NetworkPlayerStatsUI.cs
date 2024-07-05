using TMPro;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using BTG.Gameplay.GameplayObjects;
using System.Collections.Generic;
using UnityEditor.PackageManager;


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

        private List<PlayerStatsRowUI> _playerStatsRowUIList = new List<PlayerStatsRowUI>();

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

        public void AddPlayerStatRow(ulong clientId)
        {
            if (!_persistentPlayerRuntimeData.TryGetPlayer(clientId, out var persistentPlayer))
            {
                Debug.LogError("Could not find player with clientId: " + clientId);
                return;
            }

            var playerStatsRowUI = Instantiate(playerStatsRowUIPrefab, m_StatsContainer);
            _playerStatsRowUIList.Add(playerStatsRowUI);
            var playerName = persistentPlayer.NetworkNameState.Name.Value;
            playerStatsRowUI.NameText.text = playerName;
        }

        public void RemovePlayerStatRow(ulong clientId)
        {
            foreach (var playerStatsRowUI in _playerStatsRowUIList)
            {
                if (playerStatsRowUI.ClientId == clientId)
                {
                    _playerStatsRowUIList.Remove(playerStatsRowUI);
                    Destroy(playerStatsRowUI.gameObject);
                    break;
                }
            }
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
    }

}