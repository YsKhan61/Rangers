using BTG.ConnectionManagement;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;


namespace BTG.Gameplay.GameplayObjects
{
    /// <summary>
    /// Temporary cache for all active players in the game.
    /// Later create NetworkPlayerRuntimeCollectionSO to store all active players in the game.
    /// </summary>
    [RequireComponent(typeof(NetcodeHooks))]
    [RequireComponent(typeof(NetworkPlayer))]
    public class NetworkPlayerClientCache : MonoBehaviour
    {
        private static List<NetworkPlayer> ms_ActivePlayers = new List<NetworkPlayer>();
        public static List<NetworkPlayer> ActivePlayers => ms_ActivePlayers;

        private NetcodeHooks m_NetcodeHooks;
        private NetworkPlayer m_Owner;  // This is the owner of this client instance


        private void Awake()
        {
            m_NetcodeHooks = GetComponent<NetcodeHooks>();
            m_Owner = GetComponent<NetworkPlayer>();
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

            ms_ActivePlayers.Remove(m_Owner);
        }

        private void OnNetworkSpawn()
        {
            ms_ActivePlayers.Add(m_Owner);
            // LogCaching();
        }

        private void OnNetworkDespawn()
        {
            if (m_NetcodeHooks.IsServer)
            {
                Transform movementTransform = m_Owner.transform;
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
                ms_ActivePlayers.Remove(m_Owner);
            }
        }

        public static NetworkPlayer GetPlayer(ulong ownerClientId)
        {
            foreach (NetworkPlayer playerView in ms_ActivePlayers)
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
                Debug.Log("Server cached network player with client id " + m_NetcodeHooks.OwnerClientId);
            }
            else
            {
                Debug.Log("Client cached network player with client id " + m_NetcodeHooks.OwnerClientId);
            }
        }
    }
}