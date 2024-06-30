using BTG.Utilities;


namespace BTG.Actions.PrimaryAction
{
    public interface ITeslaFiring : IPrimaryAction, IUpdatable
    {
        public TeslaFiringDataSO Data { get; }
    }
}

