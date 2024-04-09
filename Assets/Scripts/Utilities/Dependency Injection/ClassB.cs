using UnityEngine;

namespace BTG.Utilities.DI
{
    /// <summary>
    /// Example class that uses dependency injection
    /// </summary>
    public class ClassB : MonoBehaviour
    {
        /// <summary>
        /// Injected dependency as a field of a monobehaviour type
        /// </summary>
        [Inject]
        ServiceA serviceA;

        /// <summary>
        /// Injected dependency as a field of a monobehaviour type
        /// </summary>
        [Inject]
        ServiceB serviceB;
        
        FactoryA factoryA;

        /// <summary>
        /// Injected dependency as a field of a ScriptableObject type
        /// </summary>
        [Inject]
        PlayerStatsSO playerStats;

        /// <summary>
        /// Injected dependency as a method parameter
        /// </summary>
        /// <param name="factoryA"></param>
        [Inject]
        public void Init(FactoryA factoryA)
        {
            this.factoryA = factoryA;
        }

        

        private void Start()
        {
            serviceA.Initialize("ServiceA initialized from ClassB");
            serviceB.Initialize("ServiceB initialized from ClassB");
            factoryA.GetServiceA().Initialize("ServiceA initialized from FactoryA");
        
            playerStats.ResetStats();
            playerStats.DeathCount.Value++;
            Debug.Log("ClassB: PlayerStatsSO DeathCount: " + playerStats.DeathCount.Value);
        }
    }

}