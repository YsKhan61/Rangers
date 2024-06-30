using BTG.Events;
using BTG.Utilities.EventBus;


namespace BTG.Actions.PrimaryAction
{
    public class ChargedFiring : ChargedFiringBase
    {
        private ProjectilePool m_Pool;
        public ChargedFiring(ChargedFiringDataSO data, ProjectilePool projectilePool) : base(data)
        {
            m_Pool = projectilePool;
        }

        public override void Destroy()
        {
            m_Pool.ClearPool();
            base.Destroy();
        }

        protected override ProjectileController CreateProjectile()
        {
            ProjectileView view = m_Pool.GetProjectile();
            ProjectileController pc = new ProjectileController(chargedFiringData, view);
            view.SetController(pc);
            return pc;
        }

        protected override void InvokeShootAudioEvent()
        {
            EventBus<AudioEventData>.Invoke(new AudioEventData
            {
                AudioTag = chargedFiringData.Tag,
                Position = actor.FirePoint.position
            });
        }
    }
}