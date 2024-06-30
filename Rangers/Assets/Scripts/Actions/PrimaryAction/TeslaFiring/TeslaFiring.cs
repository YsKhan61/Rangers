using BTG.Events;
using BTG.Utilities.EventBus;


namespace BTG.Actions.PrimaryAction
{
    public class TeslaFiring : TeslaFiringBase
    {
        private TeslaBallPool m_Pool;

        public TeslaFiring(TeslaFiringDataSO data, TeslaBallPool pool) : base(data)
        {
            m_Pool = pool;
        }

        protected override void SpawnBall()
        {
            m_BallInCharge = m_Pool.GetTeslaBall();
            m_BallInCharge.SetTeslaFiring(this);
        }

        protected override void InvokeShootAudioEvent()
        {
            EventBus<AudioEventData>.Invoke(new AudioEventData
            {
                AudioTag = teslaFringData.Tag,
                Position = actor.FirePoint.position
            });
        }
    }
}

