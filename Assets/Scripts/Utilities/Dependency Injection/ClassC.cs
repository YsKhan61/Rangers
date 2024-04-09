using UnityEngine;

namespace BTG.Utilities.DI
{
    /// <summary>
    /// Example class that is not a MonoBehaviour and uses dependency injection.
    /// When a class is not a MonoBehaviour, it must be injected manually.
    /// A provider or any other class can create an instance of this class and inject its dependencies using Injector.
    /// </summary>
    public class ClassC
    {
        /// <summary>
        /// Injected dependency as a field of monobehaviour type
        /// </summary>
        [Inject]
        ClassA classA;


        /// <summary>
        /// Injected dependency as a field of a ScriptableObject type
        /// </summary>
        [Inject]
        PlayerStatsSO playerStats;

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

            playerStats.ResetStats();
            playerStats.DeathCount.Value++;
            Debug.Log("ClassB: PlayerStatsSO DeathCount: " + playerStats.DeathCount.Value);
        }
    }

}