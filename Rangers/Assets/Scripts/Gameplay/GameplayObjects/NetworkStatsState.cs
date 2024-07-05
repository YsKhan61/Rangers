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

        public override void OnNetworkSpawn()
        {
            mn_Kills.OnValueChanged += OnKillsValueChanged;
            mn_Deaths.OnValueChanged += OnDeathsValueChanged;
        }

        public override void OnNetworkDespawn()
        {
            mn_Kills.OnValueChanged -= OnKillsValueChanged;
            mn_Deaths.OnValueChanged -= OnDeathsValueChanged;
        }

        public void IncrementKills() => mn_Kills.Value++;

        public void IncrementDeaths() => mn_Deaths.Value++;

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
