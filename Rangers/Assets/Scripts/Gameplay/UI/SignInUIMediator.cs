using BTG.UnityServices.Auth;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace BTG.Gameplay.UI
{
    /// <summary>
    /// Used to handle the UI for signing in.
    /// </summary>
    public class SignInUIMediator : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Button _signInWithPlayerAccountButton;

        [SerializeField]
        private Button _signInAsGuestButton;

        [SerializeField]
        private GameObject _signInSpinner;

        private AuthenticationServiceFacade _authServiceFacade;

        private void Awake()
        {
            ShowPanel();
        }

        [Inject]
        private void InjectDependencies(
            AuthenticationServiceFacade authServiceFacade)
        {
            _authServiceFacade = authServiceFacade;
        }

        /// <summary>
        /// Called from Button UI element. Attempts to sign in with Unity Player Account.
        /// </summary>
        public void SignInWithUnityPlayerAccount()
        {
            SignInAsync(AccountType.UnityPlayerAccount);

        }

        /// <summary>
        /// Called from Button UI element. Attempts to sign in as a guest.
        /// </summary>
        public void SignInAsGuest()
        {
            SignInAsync(AccountType.GuestAccount);
        }

        internal void ConfigurePanelOnSignInFailed()
        {
            _signInWithPlayerAccountButton.interactable = true;
            _signInAsGuestButton.interactable = true;
        }

        internal void ShowPanel()
        {
            _signInWithPlayerAccountButton.interactable = true;
            _signInAsGuestButton.interactable = true;

            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.interactable = true;
        }

        internal void HidePanel()
        {
            _canvasGroup.gameObject.SetActive(false);
            _canvasGroup.interactable = false;
        }    

        private async void SignInAsync(AccountType accountType)
        {
            _signInWithPlayerAccountButton.interactable = false;
            _signInAsGuestButton.interactable = false;
            _signInSpinner.SetActive(true);
            _authServiceFacade.SignOut(true);
            _authServiceFacade.ClearCachedSessionToken();

            try
            {
                await _authServiceFacade.InitializeAndSubscribeToUnityServicesAsync();
                _authServiceFacade.LinkAccount = false;
                await _authServiceFacade.SignInAsync(accountType);
            }
            catch
            {
                _signInWithPlayerAccountButton.interactable = true;
                _signInAsGuestButton.interactable = true;
            }
            finally
            {
                _signInSpinner.SetActive(false);
            }
        }
    }
}