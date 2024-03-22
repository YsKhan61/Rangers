using UnityEngine;


namespace BTG.UI
{
    public class GameMenuUI : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}

