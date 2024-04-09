using UnityEngine;

namespace BTG.Utilities.DI
{
    public class ClassA : MonoBehaviour
    {
        ServiceA serviceA;

        [Inject]
        IEnvironmentSystem environmentSystem;

        [Inject]
        public void Init(ServiceA serviceA)
        {
            this.serviceA = serviceA;
        }

        void Start()
        {
            serviceA.Initialize("ServiceA initialized from Class A");
            environmentSystem.Initialize();

            ClassC classC = new ClassC();
            Injector.Instance.Inject(classC);
            classC.Initialize();
        }
    }

}