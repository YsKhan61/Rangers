using UnityEngine;

namespace BTG.Utilities.DI
{
    public class ClassB : MonoBehaviour
    {

        [Inject]
        ServiceA serviceA;

        [Inject]
        ServiceB serviceB;
        
        FactoryA factoryA;

        [Inject]
        PlayerStatsSO playerStats;

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
            Debug.Log("PlayerStatsSO DeathCount: " + playerStats.DeathCount.Value);
        }
    }

}