using BTG.Utilities.EventBus;

namespace BTG.Events
{
    public class KillDeathEventData : IEvent
    {
        public ulong KillerClientId { get; }
        public ulong VictimClientId { get; }

        public KillDeathEventData(ulong killerClientId, ulong victimClientId)
        {
            KillerClientId = killerClientId;
            VictimClientId = victimClientId;
        }
    }
}
