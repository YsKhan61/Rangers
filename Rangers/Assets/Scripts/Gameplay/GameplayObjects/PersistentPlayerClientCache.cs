using BTG.ConnectionManagement;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.BossRoom;
using Unity.Multiplayer.Samples.Utilities;
using UnityEngine;


namespace BTG.Gameplay.GameplayObjects
{
    /// <summary>
    /// Temporary cache for all active players in the game.
    /// Later use PersistentPlayerRuntimeCollectionSO to store all active players in the game.
    /// </summary>
    [RequireComponent(typeof(NetcodeHooks))]
    [RequireComponent(typeof(PersistentPlayer))]
    public class PersistentPlayerClientCache : MonoBehaviour
    {
        private static List<PersistentPlayer> ms_ActivePlayers = new List<PersistentPlayer>();
        public static List<PersistentPlayer> ActivePlayers => ms_ActivePlayers;

        private NetcodeHooks m_NetcodeHooks;
        private PersistentPlayer m_Owner;  // This is the owner of this client instance


        private void Awake()
        {
            m_NetcodeHooks = GetComponent<NetcodeHooks>();
            m_Owner = GetComponent<PersistentPlayer>();
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

            LogCaching();
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

        public static PersistentPlayer GetPlayer(ulong ownerClientId)
        {
            foreach (PersistentPlayer playerView in ms_ActivePlayers)
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
                Debug.Log("Server cached persistent player with client id " + m_NetcodeHooks.OwnerClientId);
            }
            else
            {
                Debug.Log("Client cached persistent player with client id " + m_NetcodeHooks.OwnerClientId);
            }
        }
    }
}