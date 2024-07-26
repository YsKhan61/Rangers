using BTG.ConnectionManagement;
using BTG.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace BTG.Gameplay.UI
{
    public class UIQuitPanel : MonoBehaviour
    {
        private enum QuitMode
        {
            ReturnToMainMenu,
            QuitApplication         // Quit can only be possible from the StartMainMenu panel of MainMenu Scene
        }

        [SerializeField] private QuitMode _quitMode = QuitMode.ReturnToMainMenu;

        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        [Inject]
        private ConnectionManager _connectionManager;

        [Inject]
        private SceneNameListSO _sceneNameListSO;

        [Inject]
        private IPublisher<QuitApplicationMessage> _quitApplicationMessagePublisher;

        private void Awake()
        {
            _confirmButton.onClick.AddListener(OnConfirmButtonClicked);
            _cancelButton.onClick.AddListener(Cancel);
        }

        private void OnDestroy()
        {
            _confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
            _cancelButton.onClick.RemoveListener(Cancel);
        }

        private void OnConfirmButtonClicked()
        {
            switch (_quitMode)
            {
                case QuitMode.ReturnToMainMenu:
                    if (_connectionManager)
                    {
                        _connectionManager.RequestShutdown();
                    }
                    else
                    {
                        Debug.LogError("ConnectionManager not found!");
                    }
                    break;
                case QuitMode.QuitApplication:
                    _quitApplicationMessagePublisher.Publish(new QuitApplicationMessage());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            gameObject.SetActive(false);
        }

        private void Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}
