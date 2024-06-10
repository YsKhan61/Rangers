using BTG.Gameplay.Configuration;
using BTG.UnityServices.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace BTG.Gameplay.UI
{
    /// <summary>
    /// Used to handle the Start Menu UI
    /// </summary>
    public class StartMenuUIMediator : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TMP_InputField _playerNameInputField;

        [SerializeField]
        private Button _lobbyButton;

        [SerializeField]
        private Button _directIpButton;

        [SerializeField]
        private Button _linkAccountButton;

        [SerializeField]
        private Button _unlinkAccountButton;

        // also need to add delete account button.

        [SerializeField]
        private UITooltipDetector _lobbyButtonTooltipDetector;

        [SerializeField]
        private NameGenerationDataSO _nameGenerationData;

        [Inject]
        private AuthenticationServiceFacade _authenticationServiceFacade;

        private void Awake()
        {
            _lobbyButton.interactable = false;
            HidePanel();
            ShowLobbyButtonTooltip();
        }

        /// <summary>
        /// called froim Link With Unity Player Account button of Start Main Menu UI
        /// </summary>
        public void LinkAccountWithUnity()
        {
            _authenticationServiceFacade.LinkWithUnityPlayerAccountAsync();
        }

        /// <summary>
        /// Called from Unlink button of Start Main Menu UI
        /// </summary>
        public void UnlinkAccountWithUnity() 
        {
            _ = _authenticationServiceFacade.UnlinkUnityPlayerAccountAsync();
        }

        /// <summary>
        /// Called from Sign Out button of Start Main Menu UI
        /// </summary>
        public void SignOut()
        {
            _authenticationServiceFacade.SignOut(true);
            _authenticationServiceFacade.ClearCachedSessionToken();
        }

        /// <summary>
        /// Called from refresh button of Start Main Menu UI
        /// </summary>
        public void RefreshPlayerName()
        {
            _playerNameInputField.text = _nameGenerationData.GetRandomName();
        }

        /// <summary>
        /// Called from save button of Start Main Menu UI
        /// </summary>
        public void SavePlayerName()
        {
            _authenticationServiceFacade.UpdatePlayerNameAsync(_playerNameInputField.text);
        }

        internal void ConfigureStartMenuAfterSignInSuccess(string playerName)
        {
            _lobbyButton.interactable = true;
            ShowPanel();
            HideLobbyButtonTooltip();
            _playerNameInputField.text = playerName;

            if (_authenticationServiceFacade.AccountType == AccountType.GuestAccount)
            {
                if (!_linkAccountButton.gameObject.activeSelf)
                    _linkAccountButton.gameObject.SetActive(true);

                if (_unlinkAccountButton.gameObject.activeSelf)
                    _unlinkAccountButton.gameObject.SetActive(false);
            }
            else
            {
                if (_linkAccountButton.gameObject.activeSelf)
                    _linkAccountButton.gameObject.SetActive(false);

                if (!_unlinkAccountButton.gameObject.activeSelf)
                    _unlinkAccountButton.gameObject.SetActive(true);
            }
        }

        internal void ConfigureStartMenuAfterLinkAccountSuccess()
        {
            if (_linkAccountButton.gameObject.activeSelf)
                _linkAccountButton.gameObject.SetActive(false);

            if (!_unlinkAccountButton.gameObject.activeSelf)
                _unlinkAccountButton.gameObject.SetActive(true);
        }

        internal void ConfigureStartMenuAfterLinkAccountFailed()
        {
            if (!_linkAccountButton.gameObject.activeSelf)
                _linkAccountButton.gameObject.SetActive(true);

            if (_unlinkAccountButton.gameObject.activeSelf)
                _unlinkAccountButton.gameObject.SetActive(false);
        }

        internal void ConfigureStartMenuAfterUnlinkAccount()
        {
            _linkAccountButton.gameObject.SetActive(true);
            _unlinkAccountButton.gameObject.SetActive(false);
        }

        internal void ShowPanel()
        {
            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.interactable = true;    
        }

        internal void HidePanel()
        {
            _canvasGroup.gameObject.SetActive(false);
            _canvasGroup.interactable = false;
        }

        internal void ShowLobbyButtonTooltip()
        {
            // the _lobbyButtonTooltipDetector can be null if the StartMenuUIMediator is not enabled in the scene
            if (_lobbyButtonTooltipDetector != null && !_lobbyButtonTooltipDetector.enabled) _lobbyButtonTooltipDetector.enabled = true;
        }

        internal void HideLobbyButtonTooltip()
        {
            if (_lobbyButtonTooltipDetector != null && _lobbyButtonTooltipDetector.enabled) _lobbyButtonTooltipDetector.enabled = false;
        }
    }
}