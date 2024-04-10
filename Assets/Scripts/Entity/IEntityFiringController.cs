namespace BTG.Entity
{
    public interface IEntityFiringController
    {
        public event System.Action<float, float> OnPlayerCamShake;
    }
}