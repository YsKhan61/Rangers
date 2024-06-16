using BTG.ConnectionManagement;
using BTG.Gameplay.GameplayObjects;
using BTG.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace BTG.Gameplay.UI
{
    /// <summary>
    /// Handles the display of in-game messages in a message feed.
    /// </summary>
    public class LobbyChatSystem : NetworkBehaviour
    {
        const string IS_OPEN = "IsOpen";

        [SerializeField]
        List<UIMessageSlot> m_MessageSlots;

        [SerializeField]
        GameObject m_MessageSlotPrefab;

        [SerializeField]
        VerticalLayoutGroup m_VerticalLayoutGroup;

        [SerializeField]
        Button m_ChatButton;

        [SerializeField]
        TMP_InputField m_ChatInputField;

        [SerializeField]
        Button m_SendButton;

        [SerializeField]
        PersistentPlayersRuntimeCollectionSO m_PersistentPlayersRuntimeCollection;

        [SerializeField]
        Animator m_Animator;

        [Inject]
        void InjectDependencies(
            IPublisher<NetworkChatMessage> networkChatMessagePublisher,
            ISubscriber<ConnectionEventMessage> connectionEventSubscriber,
            ISubscriber<NetworkChatMessage> networkClientChatSubscriber,
            PopupManager popupManager)
        {
            m_networkChatMessagePublisher = networkChatMessagePublisher;
            m_PopupManager = popupManager;
            m_Subscriptions = new DisposableGroup();
            m_Subscriptions.Add(connectionEventSubscriber.Subscribe(OnConnectionEvent));
            m_Subscriptions.Add(networkClientChatSubscriber.Subscribe(OnChatMessageReceived));
        }

        FixedPlayerName m_OwnerClientName;
        DisposableGroup m_Subscriptions;
        Coroutine m_HideRoutine;
        PopupManager m_PopupManager;
        IPublisher<NetworkChatMessage> m_networkChatMessagePublisher;


        public override void OnNetworkSpawn()
        {
            DontDestroyOnLoad(gameObject);
            HideMessageWindow();
        }

        public override void OnNetworkDespawn()
        {
            m_Subscriptions?.Dispose();
        }

        /// <summary>
        /// Called from Send button to send a chat message.
        /// </summary>
        public void SendMessage()
        {
            if (string.IsNullOrEmpty(m_ChatInputField.text))
            {
                return;
            }

            if (string.IsNullOrEmpty(m_OwnerClientName))
            {
                // Try getting this from the LocalLobbyUser.PlayerName -> make the LocalLobbyUser a scriptable object if needed.
                m_PersistentPlayersRuntimeCollection.TryGetPlayerName(NetworkManager.Singleton.LocalClientId, out m_OwnerClientName);
            }

            SendChatMessageServerRpc(new NetworkChatMessage
            {
                Name = m_OwnerClientName,
                Message = m_ChatInputField.text
            });
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendChatMessageServerRpc(NetworkChatMessage message)
        {
            m_networkChatMessagePublisher.Publish(message);
        }

        /// <summary>
        /// Show the message window including the close button.
        /// </summary>
        public void ShowMessageWindow()
        {
            if (!m_Animator.GetBool(IS_OPEN)) m_Animator.SetBool(IS_OPEN, true);

            m_ChatButton.gameObject.SetActive(false);
        }

        public void HideMessageWindow()
        {
            if (m_Animator.GetBool(IS_OPEN)) m_Animator.SetBool(IS_OPEN, false);

            m_ChatButton.gameObject.SetActive(true);
        }

        public void ResetHideRoutine()
        {
            if (m_Animator.GetBool(IS_OPEN) && m_HideRoutine != null)
            {
                StopCoroutine(m_HideRoutine);
                m_HideRoutine = StartCoroutine(HideRoutine());
            }
        }

        private void OnChatMessageReceived(NetworkChatMessage chat)
        {
            if (chat.Name != m_OwnerClientName)
            {
                PopupManager.DisplayStatus($"{chat.Name}: {chat.Message}", 2);
            }

            DisplayChat($"{chat.Name}: {chat.Message}",
                chat.Name == m_OwnerClientName);
        }

        void OnConnectionEvent(ConnectionEventMessage eventMessage)
        {
            switch (eventMessage.ConnectStatus)
            {
                case ConnectStatus.Success:
                    PopupManager.DisplayStatus($"{eventMessage.PlayerName} has joined the game!", 2);
                    break;
                case ConnectStatus.KickedByHost:
                    PopupManager.DisplayStatus($"{eventMessage.PlayerName} has been kicked by the host!", 2);
                    break;
                case ConnectStatus.ServerFull:
                case ConnectStatus.LoggedInAgain:
                case ConnectStatus.UserRequestedDisconnect:
                case ConnectStatus.GenericDisconnect:
                case ConnectStatus.IncompatibleBuildType:
                case ConnectStatus.HostEndedSession:
                    PopupManager.DisplayStatus($"{eventMessage.PlayerName} has left the game!", 2);
                    break;
            }
        }

        void DisplayChat(string text, bool isRightAlligned  = false, bool autoOpen = false, bool autoClose = false)
        {
            if (autoOpen)
            {
                ShowMessageWindow();
            }
        
            var messageSlot = GetAvailableSlot();
            messageSlot.Display(text, isRightAlligned);

            if (autoClose)
            {
                m_HideRoutine = StartCoroutine(HideRoutine());
            }
        }

        IEnumerator HideRoutine()
        {
            yield return new WaitForSeconds(3);
            HideMessageWindow();
            m_HideRoutine = null;
        }

        UIMessageSlot GetAvailableSlot()
        {
            var go = Instantiate(m_MessageSlotPrefab, m_VerticalLayoutGroup.transform);
            var messageSlot = go.GetComponentInChildren<UIMessageSlot>();
            m_MessageSlots.Add(messageSlot);
            return messageSlot;
        }
    }
}
