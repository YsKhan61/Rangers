using BTG.Events;
using BTG.Utilities.EventBus;
using System;
using Unity.Netcode;


namespace BTG.Gameplay.GameplayObjects
{
    public class NetworkStatsState : NetworkBehaviour
    {
        public event Action<ulong, int> OnKillsChanged;
        public event Action<ulong, int> OnDeathsChanged;

        private NetworkVariable<int> mn_Kills = new(writePerm: NetworkVariableWritePermission.Server, readPerm: NetworkVariableReadPermission.Everyone);
        private NetworkVariable<int> mn_Deaths = new(writePerm: NetworkVariableWritePermission.Server, readPerm: NetworkVariableReadPermission.Everyone);

        private EventBinding<KillDeathEventData> m_KillDeathEventBinding;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                m_KillDeathEventBinding = new EventBinding<KillDeathEventData>(OnKillDeathEventInvoked);
                EventBus<KillDeathEventData>.Register(m_KillDeathEventBinding);
            }
            
            mn_Kills.OnValueChanged += OnKillsValueChanged;
            mn_Deaths.OnValueChanged += OnDeathsValueChanged;
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                EventBus<KillDeathEventData>.Unregister(m_KillDeathEventBinding);
            }

            mn_Kills.OnValueChanged -= OnKillsValueChanged;
            mn_Deaths.OnValueChanged -= OnDeathsValueChanged;
        }

        public void IncrementKills() => mn_Kills.Value++;

        public void IncrementDeaths() => mn_Deaths.Value++;

        private void OnKillDeathEventInvoked(KillDeathEventData data)
        {
            if (data.KillerClientId == OwnerClientId)
            {
                IncrementKills();
            }
            else if (data.VictimClientId == OwnerClientId)
            {
                IncrementDeaths();
            }
        }

        private void OnKillsValueChanged(int previousValue, int newValue)
        {
            OnKillsChanged?.Invoke(OwnerClientId, newValue);
        }

        private void OnDeathsValueChanged(int previousValue, int newValue)
        {
            OnDeathsChanged?.Invoke(OwnerClientId, newValue);
        }
    }
}
