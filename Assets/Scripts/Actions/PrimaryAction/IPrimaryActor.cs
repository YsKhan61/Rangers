using BTG.Utilities;
using UnityEngine;


namespace BTG.Actions.PrimaryAction
{
    public interface IPrimaryActor :ITransform
    {
        public bool IsPlayer { get; }
        public Transform FirePoint { get; }
        public float CurrentMoveSpeed { get; }
        public IPrimaryAction PrimaryAction { get; }

        public void StartPrimaryFire();
        public void StopPrimaryFire();
    }
}