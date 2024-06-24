using BTG.Factory;


namespace BTG.Actions.PrimaryAction
{
    public abstract class PrimaryActionFactorySO : FactorySO<IPrimaryAction>
    {
        public abstract IPrimaryAction GetNetworkItem();
    }
}