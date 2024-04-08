using UnityEngine;

namespace BTG.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataSO")]
    public class PlayerDataSO : ScriptableObject
    {
        [SerializeField]
        PlayerView m_Prefab;
        public PlayerView Prefab => m_Prefab;
    }
}