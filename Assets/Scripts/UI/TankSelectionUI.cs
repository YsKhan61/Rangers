using BTG.Utilities;
using UnityEngine;

namespace BTG.UI
{ 
    public class TankSelectionUI : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI panel of tank selection")]
        private GameObject m_Panel;

        [SerializeField]
        private IntDataSO m_TankIDSelectedData;

        [SerializeField]
        private IntDataSO m_PlayerDeathData;

        private void OnEnable() => m_PlayerDeathData.OnValueChanged += OnPlayerDeath;

        private void Start() => ShowPanel();

        private void OnDisable() => m_PlayerDeathData.OnValueChanged -= OnPlayerDeath;

        public void TankIDSelect(int id)
        {
            m_TankIDSelectedData.Value = id;
            HidePanel();
        }

        private void ShowPanel() => m_Panel.SetActive(true);

        private void HidePanel() => m_Panel.SetActive(false);

        private void OnPlayerDeath(int _) => ShowPanel();
    }
}
