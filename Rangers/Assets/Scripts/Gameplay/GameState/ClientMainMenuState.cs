using BTG.Gameplay.UI;
using BTG.UnityServices.Auth;
using BTG.UnityServices.Lobbies;
using BTG.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BTG.Gameplay.GameState
{
    /// <summary>
    /// Game logic that runs when sitting at the MainMenu. This is likely to be "nothing", as no game has been started.
    /// But it is nonetheless important to have a game state, as the GameStateBehaviour system requires that all scenes have states.
    /// </summary>
    /// <remarks>
    /// OnNetworkSpawn() won't ever run, because there is no network connection at the main menu screen.
    /// Fortunately we know you are a client, because all players are client when sitting at the main menu screen.
    /// </remarks>
    public class ClientMainMenuState : GameStateBehaviour
    {
        [SerializeField]
        private SignInUIMediator _signInUIMediator;

        [SerializeField]
        private StartMenuUIMediator _startMenuUIMediator;

        [SerializeField]
        private LobbyUIMediator _lobbyUIMediator;

        [SerializeField]
        private IPUIMediator _ipUIMediator;

        [SerializeField]
        private GameObject _signInSpinner;

        [Inject]
        private SceneNameListSO _sceneNameList;

        private AuthenticationServiceFacade _authServiceFacade;
        private LocalLobbyUser _localLobbyUser;
        private LocalLobby _localLobby;

        public override GameState ActiveState => GameState.MainMenu;

        private string _profileName;
        
        
        protected override void Awake()
        {
            base.Awake();

            _lobbyUIMediator.Hide();
            _signInSpinner.SetActive(false);

            if (string.IsNullOrEmpty(Application.cloudProjectId))
            {
                OnAuthSignInFailed();
                return;
            }

            _ = _authServiceFacade.InitializeAndSubscribeToUnityServicesAsync();
        }

        protected override void Start()
        {
            base.Start();

            _authServiceFacade.OnAuthSignInSuccess += OnAuthSignInSuccess;
            _authServiceFacade.OnAuthSignInFailed += OnAuthSignInFailed;
            _authServiceFacade.OnAuthSignedOutSuccess += OnAuthSignedOutSuccess;
            _authServiceFacade.OnLinkedInWithUnitySuccess += OnLinkSuccess;
            _authServiceFacade.OnLinkedInWithUnityFailed += OnLinkFailed;
            _authServiceFacade.OnUnlinkFromUnitySuccess += OnUnlinkSuccess;
            _authServiceFacade.OnAccountNameUpdateSuccess += UpdateNameSuccess;
            _authServiceFacade.OnAccountNameUpdateFailed += UpdateNameFailed;

            Application.wantsToQuit += OnApplicationWantsToQuit;

            ConfigureMainMenuAtStart();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _authServiceFacade.OnAuthSignInSuccess -= OnAuthSignInSuccess;
            _authServiceFacade.OnAuthSignInFailed -= OnAuthSignInFailed;
            _authServiceFacade.OnAuthSignedOutSuccess -= OnAuthSignedOutSuccess;
            _authServiceFacade.OnLinkedInWithUnitySuccess -= OnLinkSuccess;
            _authServiceFacade.OnLinkedInWithUnityFailed -= OnLinkFailed;
            _authServiceFacade.OnUnlinkFromUnitySuccess -= OnUnlinkSuccess;
            _authServiceFacade.OnAccountNameUpdateSuccess -= UpdateNameSuccess;
            _authServiceFacade.OnAccountNameUpdateFailed -= UpdateNameFailed;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponent(_lobbyUIMediator);
            builder.RegisterComponent(_ipUIMediator);
        }

        [Inject]
        private void AddDependencies(
            AuthenticationServiceFacade authServiceFacade,
            LocalLobbyUser localLobbyUser,
            LocalLobby localLobby)
        {
            _authServiceFacade = authServiceFacade;
            _localLobbyUser = localLobbyUser;
            _localLobby = localLobby;
        }

        /// <summary>
        /// UI Button callback to start the game in single player mode.
        /// </summary>
        public void StartSinglePlayerMode()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneNameList.SinglePlayerScene);
        }

        public void OnLobbyStartButtonClicked()
        {
            _lobbyUIMediator.ToggleJoinLobbyUI();
            _lobbyUIMediator.Show();
        }

        public void OnDirectIPButtonClicked()
        {
            _lobbyUIMediator.Hide();
            _ipUIMediator.Show();
        }


        /// <summary>
        /// Temporary method to configure the main menu UI at start.
        /// If the player just starts the game, then show SignIn Panel, 
        /// if the player is returning back from lobby/game scene, then show Start Main Menu Panel.
        /// </summary>
        private async void ConfigureMainMenuAtStart()
        {
            if (_authServiceFacade.IsAuthorizedToAuthenticationService())
            {
                _signInUIMediator.HidePanel();
                string savedName = await _authServiceFacade.GetPlayerName();
                _startMenuUIMediator.ConfigureStartMenuAfterSignInSuccess(savedName);
            }
            else
            {
                _signInUIMediator.ShowPanel();
            }
        }

        private async void OnAuthSignInSuccess()
        {
            _signInSpinner.SetActive(false);
            _signInUIMediator.HidePanel();

            string savedName = await _authServiceFacade.GetPlayerName();
            _startMenuUIMediator.ConfigureStartMenuAfterSignInSuccess(savedName);

            Debug.Log($"Signed in. Unity Player ID {_authServiceFacade.GetPlayerId()}");
            Debug.Log($"Signed in. Unity Player Name {savedName}");

            UpdateLocalLobbyUser();

            // The local lobby user object will be hooked into UI before the LocalLobby is populated during lobby join, so the LocalLobby must know about it already
            // when that happens.
            _localLobby.AddUser(_localLobbyUser);

            PopupManager.DisplayStatus("Signed in success!", 3);
        }

        private void OnAuthSignedOutSuccess()
        {
            if (_signInSpinner) _signInSpinner.SetActive(false); // This can be null if the scene is being unloaded on play mode stop.

            _startMenuUIMediator?.ShowLobbyButtonTooltip();
            _startMenuUIMediator?.HidePanel();
            _signInUIMediator?.ShowPanel();

            PopupManager.DisplayStatus("Signed out success!", 3);
        }

        private void OnAuthSignInFailed()
        {
            if (_signInSpinner)
            {
                _signInSpinner.SetActive(false);
            }

            _signInUIMediator.ConfigurePanelOnSignInFailed();

            PopupManager.DisplayStatus("Sign in failed!", 2);
        }

        private void OnLinkSuccess()
        {
            _startMenuUIMediator.ConfigureStartMenuAfterLinkAccountSuccess();

            PopupManager.DisplayStatus("Link account success!", 3);
        }

        private void OnLinkFailed()
        {
            if (_signInSpinner)
            {
                _signInSpinner.SetActive(false);
            }

            _startMenuUIMediator.ConfigureStartMenuAfterLinkAccountFailed();

            PopupManager.DisplayStatus("Link account failed!", 2);
        }

        private void OnUnlinkSuccess()
        {
            _startMenuUIMediator.ConfigureStartMenuAfterUnlinkAccount();
            PopupManager.DisplayStatus("Unlink account success!", 3);
        }

        private void UpdateNameSuccess()
        {
            // Updating LocalLobbyUser and LocalLobby
            _localLobby.RemoveUser(_localLobbyUser);
            UpdateLocalLobbyUser();
            _localLobby.AddUser(_localLobbyUser);

            PopupManager.DisplayStatus("Name update success!", 3);
        }

        private void UpdateNameFailed()
        {
            PopupManager.DisplayStatus("Name update failed!", 2);
        }

        private async void UpdateLocalLobbyUser()
        {
            _localLobbyUser.ID = _authServiceFacade.GetPlayerId();

            string playerName = await _authServiceFacade.GetPlayerName();

            // trim the player name from '#' character
            int hashIndex = playerName.IndexOf('#');
            if (hashIndex != -1)
            {
                playerName = playerName[..hashIndex];
            }

            _localLobbyUser.PlayerName = playerName[..Mathf.Min(10, playerName.Length)];
        }

        private bool OnApplicationWantsToQuit()
        {
            Application.wantsToQuit -= OnApplicationWantsToQuit;
            _authServiceFacade.SignOut(true);
            _authServiceFacade.UnsubscribeFromAuthenticationEvents();
            return true;
        }
    }
}
