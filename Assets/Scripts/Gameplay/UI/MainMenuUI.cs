using UnityEngine;


namespace BTG.Gameplay.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private string _gameSceneName = "Game";

        public void StartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_gameSceneName);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}

