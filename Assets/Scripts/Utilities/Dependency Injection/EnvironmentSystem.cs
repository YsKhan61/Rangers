using UnityEngine;

namespace BTG.Utilities.DI
{
    public interface IEnvironmentSystem
    {
        IEnvironmentSystem ProvideEnvironmentSystem();
        public void Initialize();
    }

    public class EnvironmentSystem : MonoBehaviour, IDependencyProvider, IEnvironmentSystem
    {
        [Provide]
        IEnvironmentSystem IEnvironmentSystem.ProvideEnvironmentSystem()
        {
            return this;
        }

        public void Initialize()
        {
            // Register all dependencies here
            Debug.Log("EnvironmentSystem.Initialize() called");
        }
    }

}