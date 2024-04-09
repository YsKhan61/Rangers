namespace BTG.Utilities.DI
{
    /// <summary>
    /// Example class that is not a MonoBehaviour and uses dependency injection
    /// </summary>
    public class ClassC
    {
        [Inject]
        ClassA classA;

        public void Initialize()
        {
            if (classA == null)
            {
                ColorDebug.LogInRed("ClassA is null");
            }
            else
            {
                ColorDebug.LogInGreen("ClassA is found");
            }
        }
    }

}