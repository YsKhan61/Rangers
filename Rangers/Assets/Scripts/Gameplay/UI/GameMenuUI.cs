using UnityEngine;


namespace BTG.Gameplay.UI
{
    public class GameMenuUI : MonoBehaviour
    {
        [SerializeField]
        private string m_MainMenuSceneName = "MainMenu";

        /// <summary>
        /// Go to the main menu scene
        /// </summary>
        public void GoToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(m_MainMenuSceneName);
        }
    }
}

