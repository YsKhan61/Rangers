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
    }
}