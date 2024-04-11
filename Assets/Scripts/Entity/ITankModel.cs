namespace BTG.Entity
{
    public interface IEntityTankModel
    {
        public bool IsPlayer { get; set; }
        public int MaxHealth { get; }
        public int MaxSpeed { get; }
        public int RotateSpeed { get; }
        public float Acceleration { get; }

    }

}