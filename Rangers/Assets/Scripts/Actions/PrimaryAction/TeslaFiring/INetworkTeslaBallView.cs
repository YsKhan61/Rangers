using BTG.Utilities;


namespace BTG.Actions.PrimaryAction
{
    public interface INetworkTeslaBallView : ITeslaBallView, INetworkFiringView
    {
        public void SetActorOwnerClientId(ulong clientId);
    }

}

