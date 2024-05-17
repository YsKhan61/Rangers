using BTG.Utilities;
using UnityEngine;

namespace BTG.Player
{
    /// <summary>
    /// A scriptable object that holds the data for the player
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataSO")]
    public class PlayerDataSO : ScriptableObject
    {
        [SerializeField, Tooltip("The layer mask that will be used to mark the damage collider of player")] 
        private int m_SelfLayer;
        /// <summary>
        /// The layer that will be used to mark the damage collider of player
        /// </summary>
        public int SelfLayer => m_SelfLayer;

        [SerializeField, Tooltip("The layer mask that will be used to mark the damage collider of enemy")] 
        private int m_OppositionLayer;
        /// <summary>
        /// The layer that will be used to mark the damage collider of enemy
        /// </summary>
        public int OppositionLayerMask => 1 << m_OppositionLayer;

        [SerializeField, Tooltip("The prefab of PlayerView")]
        private PlayerView m_Prefab;
        /// <summary>
        /// The prefab of PlayerView
        /// </summary>
        public PlayerView Prefab => m_Prefab;

        [SerializeField, Tooltip("Event Channel which will be raised when player's health is updated")]
        private IntIntEventChannelSO m_OnPlayerHealthUpdated;
        /// <summary>
        /// Event Channel which will be raised when player's health is updated
        /// </summary>
        public IntIntEventChannelSO OnPlayerHealthUpdated => m_OnPlayerHealthUpdated;

        [SerializeField, Tooltip("Event Channel which will be raised when Entity's Ultimate is assigned")]
        private TagEventChannelSO m_OnUltimateAssigned;
        /// <summary>
        /// Event Channel which will be raised when Entity's Ultimate is assigned
        /// </summary>
        public TagEventChannelSO OnUltimateAssigned => m_OnUltimateAssigned;

        [SerializeField, Tooltip("Event Channel which will be raised when Entity's Ultimate charge amount is updated")]
        private IntEventChannelSO m_OnUltimateChargeUpdated;
        /// <summary>
        /// Event Channel which will be raised when Entity's Ultimate charge amount is updated
        /// </summary>
        public IntEventChannelSO OnUltimateChargeUpdated => m_OnUltimateChargeUpdated;

        [SerializeField, Tooltip("Event Channel which will be raised when Entity's Ultimate is fully charged")]
        private VoidEventChannelSO m_OnUltimateFullyCharged;
        /// <summary>
        /// Event Channel which will be raised when Entity's Ultimate is fully charged
        /// </summary>
        public VoidEventChannelSO OnUltimateFullyCharged => m_OnUltimateFullyCharged;

        [SerializeField, Tooltip("Event Channel which will be raised when Entity's Ultimate is executed")]
        private VoidEventChannelSO m_OnUltimateExecuted;
        /// <summary>
        /// Event Channel which will be raised when Entity's Ultimate is executed
        /// </summary>
        public VoidEventChannelSO OnUltimateExecuted => m_OnUltimateExecuted;
    
    }
}