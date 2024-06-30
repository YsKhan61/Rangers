using BTG.Effects;
using BTG.Utilities;
using UnityEngine;
using UnityEngine.Serialization;


namespace BTG.Actions.PrimaryAction
{
    [CreateAssetMenu(fileName = "ChargedFiringData", menuName = "ScriptableObjects/PrimaryAction/ChargedFiringDataSO")]
    public class ChargedFiringDataSO : PrimaryActionDataSO
    {
        [SerializeField, Tooltip("This view is for singleplayer")]
        ProjectileView m_ViewPrefab;
        public ProjectileView ViewPrefab => m_ViewPrefab;

        [SerializeField, Tooltip("The view for multiplayer")]
        NetworkProjectileView m_NetworkViewPrefab;
        public NetworkProjectileView NetworkViewPrefab => m_NetworkViewPrefab;

        [SerializeField, Tooltip("Minimum initial speed of the projectile at least charge")]
        float m_MinInitialSpeed;
        public float MinInitialSpeed => m_MinInitialSpeed;

        [SerializeField, Tooltip("Maximum initial speed of the projectile at most charge")]
        float m_MaxInitialSpeed;
        public float MaxInitialSpeed => m_MaxInitialSpeed;
    }
}

