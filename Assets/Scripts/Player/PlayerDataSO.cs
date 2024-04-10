using BTG.Utilities;
using UnityEngine;

namespace BTG.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataSO")]
    public class PlayerDataSO : ScriptableObject
    {
        [SerializeField, Tooltip("The layer that will be used to mark the damage collider of player")] 
        int m_SelfLayer;
        public int SelfLayer => m_SelfLayer;
        
        [SerializeField, Tooltip("The layer that will be used to mark the damage collider of enemy")] 
        int m_OppositionLayer;
        public int OppositionLayer => m_OppositionLayer;

        [SerializeField]
        PlayerView m_Prefab;
        public PlayerView Prefab => m_Prefab;

        [SerializeField]
        IntIntEventChannelSO m_OnPlayerHealthUpdated;
        public IntIntEventChannelSO OnPlayerHealthUpdated => m_OnPlayerHealthUpdated;

        [SerializeField]
        StringEventChannelSO m_OnUltimateAssigned;
        public StringEventChannelSO OnUltimateAssigned => m_OnUltimateAssigned;

        [SerializeField]
        IntEventChannelSO m_OnUltimateChargeUpdated;
        public IntEventChannelSO OnUltimateChargeUpdated => m_OnUltimateChargeUpdated;

        [SerializeField]
        VoidEventChannelSO m_OnUltimateFullyCharged;
        public VoidEventChannelSO OnUltimateFullyCharged => m_OnUltimateFullyCharged;

        [SerializeField]
        VoidEventChannelSO m_OnUltimateExecuted;
        public VoidEventChannelSO OnUltimateExecuted => m_OnUltimateExecuted;

        [SerializeField]
        FloatFloatEventChannelSO m_OnCameraShake;
        public FloatFloatEventChannelSO OnCameraShake => m_OnCameraShake;
    
    }
}