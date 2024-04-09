using UnityEngine;

namespace BTG.Utilities.DI
{
    /// <summary>
    /// Example monobehaviour class that uses dependency injection
    /// </summary>
    public class ClassA : MonoBehaviour
    {
        ServiceA serviceA;

        /// <summary>
        /// Injected dependency as a field
        /// </summary>
        [Inject]
        IEnvironmentSystem environmentSystem;

        /// <summary>
        /// Injected dependency as a method parameter
        /// </summary>
        /// <param name="serviceA"></param>
        [Inject]
        public void Init(ServiceA serviceA)
        {
            this.serviceA = serviceA;
        }

        void Start()
        {
            serviceA.Initialize("ServiceA initialized from Class A");
            environmentSystem.Initialize();

            ClassC classC = new();
            Injector.Instance.Inject(classC);       // Inject dependencies into the instance
            classC.Initialize();
        }
    }

}