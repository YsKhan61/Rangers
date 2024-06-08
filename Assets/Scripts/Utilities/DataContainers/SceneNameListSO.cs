using UnityEngine;


namespace BTG.Utilities
{
    [CreateAssetMenu(fileName = "SceneNameListSO", menuName = "ScriptableObjects/SceneNameListSO")]
    public class SceneNameListSO : ScriptableObject
    {
        public string StartUpScene;
        public string MainMenuScene;
        public string CharSelectScene;
        public string SinglePlayerScene;
        public string MultiplayerScene;
    }
}