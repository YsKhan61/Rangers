using UnityEngine;

namespace BTG.Utilities.DI
{
    public class Provider : MonoBehaviour, IDependencyProvider 
    {
        [SerializeField]
        PlayerStatsSO playerStats;

        [SerializeField]
        ClassA classA;

        [Provide]
        public ClassA ProvideClassA() => classA;

        [Provide]
        public ServiceA ProvideServiceA() => new ServiceA();

        [Provide]
        public ServiceB ProvideServiceB() => new ServiceB();

        [Provide]
        public FactoryA ProvideFactoryA() => new FactoryA();

        [Provide]
        public PlayerStatsSO ProvidePlayerStats() => playerStats;
    }

    public class ServiceA
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceA initialized with message: {message}");
        }
    }

    public class ServiceB
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"ServiceB initialized with message: {message}");
        }
    }

    public class FactoryA
    {
        ServiceA cachedServiceA;

        public ServiceA GetServiceA()
        {
            cachedServiceA ??= new ServiceA();

            return cachedServiceA;
        }
    }
}