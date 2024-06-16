using BTG.ConnectionManagement;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;


namespace BTG.Gameplay.GameplayObjects
{
    [RequireComponent(typeof(NetcodeHooks))]
    [RequireComponent(typeof(NetworkPlayerView))]
    public class NetworkPlayerViewClientCache : MonoBehaviour
    {
        private static List<NetworkPlayerView> ms_ActivePlayers = new List<NetworkPlayerView>();
        public static List<NetworkPlayerView> ActivePlayers => ms_ActivePlayers;

        private NetcodeHooks m_NetcodeHooks;
        private NetworkPlayerView m_NetworkPlayerView;


        private void Awake()
        {
            m_NetcodeHooks = GetComponent<NetcodeHooks>();
            m_NetworkPlayerView = GetComponent<NetworkPlayerView>();
        }

        private void OnEnable()
        {
            m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook += OnNetworkDespawn;
        }

        private void OnDisable()
        {
            m_NetcodeHooks.OnNetworkSpawnHook -= OnNetworkSpawn;
            m_NetcodeHooks.OnNetworkDespawnHook -= OnNetworkDespawn;
        }

        private void OnNetworkSpawn()
        {
            ms_ActivePlayers.Add(m_NetworkPlayerView);

            LogCaching();
        }

        private void OnNetworkDespawn()
        {
            if (m_NetcodeHooks.IsServer)
            {
                Transform movementTransform = m_NetworkPlayerView.transform;
                SessionPlayerData? sessionPlayerData = SessionManager<SessionPlayerData>.Instance.GetPlayerData(m_NetcodeHooks.OwnerClientId);
                if (sessionPlayerData.HasValue)
                {
                    SessionPlayerData playerData = sessionPlayerData.Value;
                    playerData.PlayerPosition = movementTransform.position;
                    playerData.PlayerRotation = movementTransform.rotation;
                    playerData.HasCharacterSpawned = true;
                    SessionManager<SessionPlayerData>.Instance.SetPlayerData(m_NetcodeHooks.OwnerClientId, playerData);
                }
            }
            else
            {
                ms_ActivePlayers.Remove(m_NetworkPlayerView);
            }
        }

        public static NetworkPlayerView GetPlayerView(ulong ownerClientId)
        {
            foreach (NetworkPlayerView playerView in ms_ActivePlayers)
            {
                if (playerView.OwnerClientId == ownerClientId)
                {
                    return playerView;
                }
            }

            return null;
        }

        private void LogCaching()
        {
            if (m_NetcodeHooks.IsServer)
            {
                Debug.Log("Server cached player with client id " + m_NetcodeHooks.OwnerClientId);
            }
            else
            {
                Debug.Log("Client cached player with client id " + m_NetcodeHooks.OwnerClientId);
            }
        }
    }
}