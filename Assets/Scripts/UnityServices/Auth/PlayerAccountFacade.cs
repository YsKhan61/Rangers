using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;

namespace BTG.UnityServices.Auth
{
    /// <summary>
    /// This class is a facade for the Unity's Player Account Service
    /// The AuthServiceFacade access this facade to interact with the Unity's Player Account Service
    /// </summary>
    public class PlayerAccountFacade
    {
        public void SubscribeToEvents()
        {
            PlayerAccountService.Instance.SignedOut += PlayerAcountSignedOut;
        }

        public void UnsubscribeFromEvents()
        {
            PlayerAccountService.Instance.SignedOut -= PlayerAcountSignedOut;
        }

        public void SignOutUnityPlayerAccount()
        {
            if (IsPlayerAccountServiceSignedIn())
            {
                PlayerAccountService.Instance.SignOut();
            }
        }

        /// <summary>
        /// Is the Player Account Service signed in
        /// </summary>
        public bool IsPlayerAccountServiceSignedIn()
        {
            return PlayerAccountService.Instance.IsSignedIn;
        }

        public string GetPlayerAccountAccessToken()
        {
            return PlayerAccountService.Instance.AccessToken;
        }

        /// <summary>
        /// Create a new Unity Authentication player with the Unity Player Accounts credentials.
        /// Sign in an existing player using the Unity Player Accounts credentials.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <remarks>https://docs.unity.com/ugs/en-us/manual/authentication/manual/unity-player-accounts</remarks>
        public async Task SignInWithUnityPlayerAccountAsync()
        {
            if (IsPlayerAccountServiceSignedIn())
            {
                Debug.Log("Player Account Service is already signed in");
                return;
            }

            try
            {
                await PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (Exception e)
            {
                Debug.LogError("Sign in with Unity Player Account failed: " + e.Message);
            }
        }

        private void PlayerAcountSignedOut()
        {
            Debug.Log("Player Account Service Signed out");
        }
    }
}

