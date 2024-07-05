using BTG.Utilities;


namespace BTG.Actions.PrimaryAction
{
    public interface INetworkProjectileView : IProjectileView, INetworkFiringView
    {
        public void SetActorOwnerClientId(ulong clientId);
    }
}

