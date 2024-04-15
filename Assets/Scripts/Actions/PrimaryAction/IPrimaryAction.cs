using BTG.Utilities;

namespace BTG.Actions.PrimaryAction
{
    public interface IPrimaryAction
    {
        public void Enable();
        public void Disable();

        public void StartAction();
        public void StopAction();
    }
}