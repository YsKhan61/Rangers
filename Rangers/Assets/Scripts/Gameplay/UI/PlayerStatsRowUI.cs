using TMPro;
using UnityEngine;


namespace BTG.Gameplay.UI
{
    public class PlayerStatsRowUI : MonoBehaviour
    {
        public ulong ClientId = ulong.MaxValue;
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI KillText;
        public TextMeshProUGUI DeathText;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }

}