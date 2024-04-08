using BTG.EventSystem;
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

        private void OnEnable()
        {
            EventService.Instance.OnBeforeAnyTankDead.AddListener(OnBeforeAnyTankDead);
        }

        private void Start()
        {
            ShowPanel();
        }

        private void OnDisable()
        {
            EventService.Instance.OnBeforeAnyTankDead.RemoveListener(OnBeforeAnyTankDead);
        }

        public void TankIDSelect(int id)
        {
            m_TankIDSelectedData.Value = id;
            EventService.Instance.OnPlayerTankSelected.InvokeEvent();
            HidePanel();
        }

        private void ShowPanel()
        {
            m_Panel.SetActive(true);
        }

        private void HidePanel()
        {
            m_Panel.SetActive(false);
        }

        private void OnBeforeAnyTankDead(bool isPlayer)
        {
            if (!isPlayer) return;

            ShowPanel();
        }
    }
}
