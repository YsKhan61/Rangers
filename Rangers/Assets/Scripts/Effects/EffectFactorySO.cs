using BTG.Factory;


namespace BTG.Effects
{
    public abstract class EffectFactorySO : FactorySO<EffectView>
    { 
        public abstract EffectDataSO Data { get; }
    }
}
