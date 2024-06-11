using BTG.Utilities;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
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
    /// This struct is used to store the access token and session token.
    /// </summary>
    public struct UserTokens
    {
        public string AccessToken;
        public string SessionToken;
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

        private IPublisher<UnityServiceErrorMessage> _unityServiceErrorMessagePublisher;
        
        private PlayerAccountFacade _playerAccountFacade;

        public bool LinkAccount;

        public AccountType AccountType { get; private set; }

        [Inject]
        public void InjectAndInitializeDependencies()
        {
            AccountType = AccountType.None;
            _playerAccountFacade = new PlayerAccountFacade();
        }


        public void SubscribeToAuthenticationEvents()
        {   
            AuthenticationService.Instance.SignedIn += AuthSignedIn;
            AuthenticationService.Instance.SignedOut += AuthSignedOut;

            PlayerAccountService.Instance.SignedIn += LinkPlayerAccountWithAuthentication;
            _playerAccountFacade.SubscribeToEvents();
        }

        public void UnsubscribeFromAuthenticationEvents()
        {
            AuthenticationService.Instance.SignedIn -= AuthSignedIn;
            AuthenticationService.Instance.SignedOut -= AuthSignedOut;

            PlayerAccountService.Instance.SignedIn -= LinkPlayerAccountWithAuthentication;
            _playerAccountFacade.UnsubscribeFromEvents();
        }

        /// <summary>
        /// Generate the initialization options for Unity Services
        /// </summary>
        /// <param name="profileName">the profile name to be set in the options</param>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/profile-management</remarks>
        public InitializationOptions GenerateInitializationOption(string profileName = null)
        {
            InitializationOptions initializationOptions = new();
            initializationOptions.SetProfile(profileName);
            return initializationOptions;
        }

        /// <summary>
        ///  Asynchronously initializes Unity Services. 
        ///  It need to be called before any other Unity Services API is used.
        ///  If already initialized, the method returns. 
        ///  In case of an exception, it publishes an error and re-throws the exception.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAndSubscribeToUnityServicesAsync(InitializationOptions options = null)
        {
            if (Unity.Services.Core.UnityServices.State == ServicesInitializationState.Initialized)
            {
                return;
            }

            try
            {
                await Unity.Services.Core.UnityServices.InitializeAsync(options);

                UnsubscribeFromAuthenticationEvents();
                SubscribeToAuthenticationEvents();

                AccountType = AccountType.None;
            }
            catch (Exception e)
            {
                PublishError("Initialization to UnityServices Error", e);
                throw;
            }
        }

        /// <summary>
        /// Sign in with third party account such as Unity Player Account, Google, Facebook etc.
        /// </summary>
        public async Task SignInAsync(AccountType accountType)
        {
            try
            {
                switch (accountType)
                {
                    case AccountType.UnityPlayerAccount:
                        await _playerAccountFacade.SignInWithUnityPlayerAccountAsync();
                        break;
                    case AccountType.GuestAccount:
                        await SignInAnonymouslyAsync();
                        break;
                }
            }
            catch (Exception e)
            {
                OnAuthSignInFailed?.Invoke();
                PublishError("Authentication Error", e);
                throw;
            }
        }

        /// <summary>
        /// Signing out resets the access token and player ID. 
        /// The Authentication service preserves the session token to allow the player to sign in to the same account in the future.
        /// If you want to sign in to a different account, clear your session token or switch profiles while you are signed out.
        /// </summary>
        /// <param name="clearCredentials">clear playerID, access token, session token</param>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/sign-out</remarks>
        public void SignOut(bool clearCredentials = false)
        {
            switch (AccountType)
            {
                case AccountType.UnityPlayerAccount:
                    _playerAccountFacade.SignOutUnityPlayerAccount();
                    break;
            }

            SignOutAuthenticationService(true);
        }

        /// <summary>
        /// Sign out from the Unity Authentication service
        /// </summary>
        /// <param name="clearCredentials"></param>
        public void SignOutAuthenticationService(bool clearCredentials = false)
        {
            if (IsSignedInToAuthenticationService())
            {
                AuthenticationService.Instance.SignOut(clearCredentials);
                AccountType = AccountType.None;
            }
        }

        /// <summary>
        /// DeleteAccountAsync() only deletes the player’s Unity Authentication account.
        /// Upon such a deletion request, you must delete all associated player data connected to the player’s Unity Authentication account and other UGS services you use.
        /// </summary>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/delete-accounts</remarks>
        public async void DeleteAccount()
        {
            try
            {
                await AuthenticationService.Instance.DeleteAccountAsync();
            }
            catch (Exception e)
            {
                Debug.LogError("Delete account failed: " + e.Message);
            }
        }

        /// <summary>
        /// Sign in the cached user account if the session token exists
        /// </summary>
        /// <remarks></remarks>
        public async Task SignInCachedUserAccount()
        {
            // Check if a cached player already exists by checking if the session token exists
            if (!IsSessionTokenExisting())
            {
                return;
            }

            try
            {
                await SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                Debug.LogError("Sign in cached user account failed: " + e.Message);
            }
        }

        public async void SignUserWithCustomTokenWithAutoRefresh()
        {
            try
            {
                if (IsSessionTokenExisting())
                {
                    await SignInCachedUserAccount();
                }
                else
                {
                    UserTokens userTokens = new();
                    AuthenticationService.Instance.ProcessAuthenticationTokens(userTokens.AccessToken, userTokens.SessionToken);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Sign in with custom token failed: " + e.Message);
            }
        }

        public string GetPlayerId()
        {
            return AuthenticationService.Instance.PlayerId;
        }

        /// <summary>
        /// Get the player name from the Authentication service
        /// </summary>
        public async Task<string> GetPlayerName()
        {
            if (!IsAuthorizedToAuthenticationService())
                return null;

            string playerName = AuthenticationService.Instance.PlayerName;
            if (!string.IsNullOrEmpty(playerName))
            {
                return playerName;
            }

            try
            {
                playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
                return playerName;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to get player name: " + e.Message);
                return null;
            }
        }

        public bool IsSignedInToAuthenticationService() => AuthenticationService.Instance.IsSignedIn;
        public bool IsSessionTokenExisting() => AuthenticationService.Instance.SessionTokenExists;
        public bool IsAuthorizedToAuthenticationService() => AuthenticationService.Instance.IsAuthorized;

        /// <summary>
        /// Update the player name
        /// </summary>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/player-name-management</remarks>
        public async void UpdatePlayerNameAsync(string name)
        {
            if (!IsSignedInToAuthenticationService())
            {
                Debug.Log("Authentication Service not signed in!");
                return;
            }
            if (string.IsNullOrEmpty(name))
            {
                Debug.Log("Player name is empty!");
                return;
            }

            try
            {
                await AuthenticationService.Instance.UpdatePlayerNameAsync(name.RemoveWhiteSpaceAndLimitLength(10));
                OnAccountNameUpdateSuccess?.Invoke();
            }
            catch (Exception e)
            {
                OnAccountNameUpdateFailed?.Invoke();
                PublishError("Authentication Error", e);
                throw;
            }
        }

        /// <summary>
        /// Link the current player account with the Unity Player Accounts credentials.
        /// </summary>
        public void LinkWithUnityPlayerAccountAsync()
        {
            if (!IsAuthorizedToAuthenticationService())
            {
                Debug.Log("Not authorized with Unity Authentication");
                return;
            }

            LinkAccount = true;

            if (_playerAccountFacade.IsPlayerAccountServiceSignedIn())
            {
                LinkPlayerAccountWithAuthentication();
            }
            else
            {
                _ = _playerAccountFacade.SignInWithUnityPlayerAccountAsync();
            }
        }

        /// <summary>
        /// Unlink the current player account from the Unity Player Accounts credentials.
        /// </summary>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/unity-player-accounts</remarks>
        public async Task UnlinkUnityPlayerAccountAsync()
        {
            if (!IsAuthorizedToAuthenticationService())
            {
                Debug.Log("Not signed in with Unity Authentication");
                return;
            }

            try
            {
                await AuthenticationService.Instance.UnlinkUnityAsync();
                AccountType = AccountType.GuestAccount;
                _playerAccountFacade.SignOutUnityPlayerAccount();
                OnUnlinkFromUnitySuccess?.Invoke();
            }
            catch (Exception e)
            {
                PublishError("Unlink Account Error", e);
            }
        }

        public void SwitchProfile(string profileName)
        {
            if (IsSignedInToAuthenticationService())
                return;

            AuthenticationService.Instance.SwitchProfile(profileName.FilterStringToLetterDigitDashUnderscoreMaxLength(30));
        }

        /// <summary>
        /// Clear the cached session token for the current profile.
        /// </summary>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/session-token-management</remarks>
        public void ClearCachedSessionToken()
        {
            if (IsSessionTokenExisting())
            {
                AuthenticationService.Instance.ClearSessionToken();
            }
        }

        /// <summary>
        /// Sign in annonmously
        /// </summary>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/use-anon-sign-in</remarks>
        private async Task SignInAnonymouslyAsync()
        {
            if (IsSignedInToAuthenticationService())
            {
                Debug.Log("Already signed in");
                return;
            }

            try
            {
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

        private void AuthSignedIn()
        {
            Debug.Log($"Signed in as {AuthenticationService.Instance.PlayerName}.");

            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

            OnAuthSignInSuccess?.Invoke();
        }

        private void AuthSignedOut()
        {
            AccountType = AccountType.None;
            OnAuthSignedOutSuccess?.Invoke();
        }

        private async void LinkPlayerAccountWithAuthentication()
        {
            if (!_playerAccountFacade.IsPlayerAccountServiceSignedIn())
            {
                Debug.Log("Player Account Service not signed in");
                return;
            }

            try
            {
                // now connect the Unity Player Account with the Unity Authentication
                string accessToken = _playerAccountFacade.GetPlayerAccountAccessToken();

                if (LinkAccount)
                {
                    await AuthenticationService.Instance.LinkWithUnityAsync(accessToken);
                    OnLinkedInWithUnitySuccess?.Invoke();
                }
                else
                {
                    await AuthenticationService.Instance.SignInWithUnityAsync(accessToken);
                }

                AccountType = AccountType.UnityPlayerAccount;
            }
            catch (Exception ex)
            {
                OnLinkedInWithUnityFailed?.Invoke();
                PublishError("Link Account Error", ex);
                _playerAccountFacade.SignOutUnityPlayerAccount();
            }
        }

        private void PublishError(string errorMessage, Exception e)
        {
            string reason = e.InnerException == null ? e.Message : $"{e.Message} ({e.InnerException.Message})";
            _unityServiceErrorMessagePublisher.Publish(new UnityServiceErrorMessage(errorMessage, reason, UnityServiceErrorMessage.Service.Authentication, e));
        }
    }
}

