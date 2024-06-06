using BTG.Utilities;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using VContainer;

namespace BTG.UnityServices.Auth
{
    /// <summary>
    /// Types of accounts that can be signed in to.
    /// </summary>
    public enum AccountType
    {
        None,
        UnityPlayerAccount,
        GuestAccount,
    }

    /// <summary>
    /// A facade to the Unity Services Authentication service.
    /// It provides a simplified interface to the Unity Services Authentication service.
    /// </summary>
    public class AuthenticationServiceFacade
    {
        public event Action OnAuthSignInSuccess;
        public event Action OnAuthSignInFailed;
        public event Action OnAuthSignedOutSuccess;
        public event Action OnLinkedInWithUnitySuccess;
        public event Action OnLinkedInWithUnityFailed;
        public event Action OnUnlinkFromUnitySuccess;
        public event Action OnAccountNameUpdateSuccess;
        public event Action OnAccountNameUpdateFailed;

        [Inject]
        private IPublisher<UnityServiceErrorMessage> _unityServiceErrorMessagePublisher;

        private bool _linkWithUnityPlayerAccount;

        public AccountType AccountType { get; private set; }

        public void SubscribeToAuthenticationEvents()
        {
            PlayerAccountService.Instance.SignedIn += SignInWithUnityPlayerAccount;
            AuthenticationService.Instance.SignedIn += AuthSignedIn;
            AuthenticationService.Instance.SignedOut += AuthSignedOut;
            AuthenticationService.Instance.SignedOut += ClearSessionToken;
        }

        public void UnsubscribeFromAuthenticationEvents()
        {
            PlayerAccountService.Instance.SignedIn -= SignInWithUnityPlayerAccount;
            AuthenticationService.Instance.SignedIn -= AuthSignedIn;
            AuthenticationService.Instance.SignedOut -= AuthSignedOut;
            AuthenticationService.Instance.SignedOut -= ClearSessionToken;
        }

        public InitializationOptions GenerateAuthenticationInitOptions(string profileName = null)
        {
            try
            {
                var initializationOptions = new InitializationOptions();
                if (!string.IsNullOrEmpty(profileName))
                {
                    initializationOptions.SetProfile(profileName);
                }

                return initializationOptions;
            }
            catch (Exception e)
            {
                PublishError("Authentication Error", e);
                throw;
            }
        }

        public async Task InitializeUnityServicesAsync()
        {
            if (Unity.Services.Core.UnityServices.State == ServicesInitializationState.Initialized)
            {
                return;
            }

            try
            {
                await Unity.Services.Core.UnityServices.InitializeAsync();
                AccountType = AccountType.None;
            }
            catch (Exception e)
            {
                PublishError("Initialization to UnityServices Error", e);
                throw;
            }
        }

        public async Task InitializeUnityServicesAsync(InitializationOptions initializationOptions)
        {
            try
            {
                await Unity.Services.Core.UnityServices.InitializeAsync(initializationOptions);
                AccountType = AccountType.None;
            }
            catch (Exception e)
            {
                PublishError("Initialization to UnityServices Error", e);
                throw;
            }
        }

        public async Task SignInWithUnityAsync()
        {
            _linkWithUnityPlayerAccount = false;

            if (PlayerAccountService.Instance.IsSignedIn)
            {
                SignInWithUnityPlayerAccount();
                return;
            }

            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (Exception e)
            {
                OnAuthSignInFailed?.Invoke();
                PublishError("Authentication Error", e);
            }
        }

        public async Task SignInAnonymously()
        {
            try
            {
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    throw new Exception("Already signed in.");
                }

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                AccountType = AccountType.GuestAccount;
            }
            catch (Exception e)
            {
                OnAuthSignInFailed?.Invoke();
                PublishError("Anonymous Sign in Error", e);
                throw;
            }
        }

        public async Task LinkAccountWithUnityAsync()
        {
            _linkWithUnityPlayerAccount = true;

            try
            {
                if (!AuthenticationService.Instance.SessionTokenExists)
                {
                    return;
                }

                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (Exception e)
            {
                OnLinkedInWithUnityFailed?.Invoke();
                _linkWithUnityPlayerAccount = false;
                PublishError("Authentication Error", e);
            }
        }

        public async Task UnlinkAccountWithUnityAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(PlayerAccountService.Instance.IdToken))
                {
                    return;
                }

                await AuthenticationService.Instance.UnlinkUnityAsync();
                OnUnlinkFromUnitySuccess?.Invoke();
            }
            catch (Exception e)
            {
                PublishError("Unlink Account Error", e);
            }
        }

        public async Task<bool> EnsurePlayerIsAuthorized()
        {
            if (AuthenticationService.Instance.IsAuthorized)
            {
                return true;
            }

            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                return true;
            }
            catch (AuthenticationException e)
            {
                PublishError("Authentication Error", e);
                return false;
            }
            catch (Exception e)
            {
                PublishError("Authentication Error", e);
                throw;
            }
        }

        public async Task UpdatePlayerNameAsync(string playerName)
        {
            if (string.IsNullOrEmpty(playerName) || !AuthenticationService.Instance.IsSignedIn)
            {
                return;
            }

            playerName = playerName.Substring(0, Math.Min(playerName.Length, 10));

            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
                OnAccountNameUpdateSuccess?.Invoke();
            }
            catch (Exception e)
            {
                OnAccountNameUpdateFailed?.Invoke();
                PublishError("Authentication Error", e);
                throw;
            }
        }

        public void ClearSessionToken()
        {
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                AuthenticationService.Instance.ClearSessionToken();
            }
        }

        public void SignOutFromAuthService(bool clearCredentials = false)
        {
            if (IsSignedIn())
            {
                AuthenticationService.Instance.SignOut(clearCredentials);
                AccountType = AccountType.None;
            }
        }

        public void SignOutFromPlayerAccountService()
        {
            PlayerAccountService.Instance.SignOut();
            AccountType = AccountType.GuestAccount;
        }

        public string GetPlayerName()
        {
            return AuthenticationService.Instance.PlayerName;
        }

        public string GetPlayerId()
        {
            return AuthenticationService.Instance.PlayerId;
        }

        public void SwitchProfile(string profileName)
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                SignOutFromAuthService(true);
            }

            AuthenticationService.Instance.SwitchProfile(profileName);
        }

        public bool IsSignedIn()
        {
            return AuthenticationService.Instance.IsSignedIn;
        }

        private async void AuthSignedIn()
        {
            await GetPlayerNameAsync();
            OnAuthSignInSuccess?.Invoke();
        }

        private void AuthSignedOut()
        {
            AccountType = AccountType.None;
            OnAuthSignedOutSuccess?.Invoke();
        }

        private async Task GetPlayerNameAsync()
        {
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                await AuthenticationService.Instance.GetPlayerNameAsync();
            }
        }

        private async void SignInWithUnityPlayerAccount()
        {
            try
            {
                if (_linkWithUnityPlayerAccount)
                {
                    await AuthenticationService.Instance.LinkWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                    OnLinkedInWithUnitySuccess?.Invoke();
                }
                else
                {
                    await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                }
                AccountType = AccountType.UnityPlayerAccount;
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                OnLinkedInWithUnityFailed?.Invoke();
                PublishError("Link Account Error", ex);
            }
            catch (Exception ex)
            {
                OnLinkedInWithUnityFailed?.Invoke();
                PublishError("Link Account Error", ex);
            }
        }

        private void PublishError(string errorMessage, Exception e)
        {
            string reason = e.InnerException == null ? e.Message : $"{e.Message} ({e.InnerException.Message})";
            _unityServiceErrorMessagePublisher.Publish(new UnityServiceErrorMessage(errorMessage, reason, UnityServiceErrorMessage.Service.Authentication, e));
        }
    }
}

