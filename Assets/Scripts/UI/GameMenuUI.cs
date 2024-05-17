using UnityEngine;


namespace BTG.UI
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

