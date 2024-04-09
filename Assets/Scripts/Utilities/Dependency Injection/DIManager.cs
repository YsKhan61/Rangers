using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BTG.Utilities.DI
{
    /// <summary>
    /// DIManager class is responsible for resolving dependencies and injecting them into objects.
    /// We can use this injector directly in the project to resolve dependencies.
    /// 
    /// 
    /// Making Injector as singleton is not necessary, 
    /// but it is a good practice to have a single instance of Injector.
    /// as it will be used to resolve dependencies.
    /// and helps in keeping track of all the dependencies.
    /// for debugging purposes.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class DIManager : Singleton<DIManager>
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    
        readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        public override void Awake()
        {
            base.Awake();

            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach ( var provider in providers )
            {
                RegisterProvider(provider);
            }

            // Find all injectable objects and inject dependencies
            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        /// <summary>
        /// The Inject method in the Injector class is responsible for injecting dependencies into objects. 
        /// It can take any custom type as a parameter and 
        /// searches for fields and methods with the InjectAttribute attribute in the object. 
        /// It then resolves the dependencies for these fields and methods and injects them into the object.
        /// If the injectable parameter is a MonoBehaviour, the method calls another overloaded version of Inject 
        /// specifically designed for MonoBehaviour objects.
        /// Otherwise, it retrieves the type of the injectable object and searches for fields and methods 
        /// with the InjectAttribute attribute.
        /// For each injectable field, the method resolves the field's type and sets the value of the field to the resolved dependency. 
        /// It also logs a debug message indicating that the field has been injected.
        /// For each injectable method, the method retrieves the required parameter types, resolves the dependencies for these parameters, 
        /// and invokes the method with the resolved instances as arguments.It also logs a debug message indicating that the method has been injected.
        /// If a dependency cannot be resolved for a field or method, an exception is thrown with an appropriate error message.
        /// </summary>
        /// <param name="injectable"></param>
        /// <exception cref="Exception"></exception>
        public void Inject(object injectable)
        {
            if (injectable is MonoBehaviour monoBehaviour)
            {
                Inject(monoBehaviour);
            }
            else
            {
                var type = injectable.GetType();
                var injectableFields = type.GetFields(BINDING_FLAGS)
                    .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

                foreach (var injectableField in injectableFields)
                {
                    var fieldType = injectableField.FieldType;
                    var resolved = Resolve(fieldType) 
                        ?? throw new Exception($"Failed to resolve {fieldType.Name} for {type.Name}");

                    injectableField.SetValue(injectable, resolved);
                    Debug.Log($"Field Injected {fieldType.Name} into {type.Name}");
                }

                var injectableMethods = type.GetMethods(BINDING_FLAGS)
                    .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

                foreach (var injectableMethod in injectableMethods)
                {
                    var requiredParameters = injectableMethod.GetParameters()
                        .Select(parameter => parameter.ParameterType)
                        .ToArray();

                    var resolvedInstances = requiredParameters
                        .Select(Resolve)
                        .ToArray();

                    if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                    {
                        throw new Exception($"Failed to resolve parameters for {injectableMethod.Name} in {type.Name}");
                    }

                    injectableMethod.Invoke(injectable, resolvedInstances);
                    Debug.Log($"Method Injected {string.Join(", ", requiredParameters.Select(p => p.Name))} into {type.Name}");
                }
            }
        }

        /// <summary>
        /// This method is used to provide an instance of the object of the specified type by creating it.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object ProvideType(Type type)
        {
            if (registry.ContainsKey(type))
            {
                Debug.LogWarning($"Object of type {type.Name} is already registered");
                return registry[type];
            }

            var instance = Activator.CreateInstance(type);
            registry.Add(type, instance);
            return instance;
        }

        /// <summary>
        /// This method is used to provide an instance of the object of the specified type given by the parameter.
        /// </summary>
        /// <typeparam name="T">The type of the object to be registered</typeparam>
        /// <param name="instance">The instance to be registered</param>
        public void Provide<T>(T instance)
        {
            var type = typeof(T);
            if (registry.ContainsKey(type))
            {
                Debug.LogWarning($"Object of type {type.Name} is already registered");
                return;
            }

            registry.Add(type, instance);
        }
        

        private void Inject(MonoBehaviour injectable)
        {
            var type = injectable.GetType();
            var injectableFields = type.GetFields(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolved = Resolve(fieldType);

                if (resolved == null)
                {
                    throw new Exception($"Failed to resolve {fieldType.Name} for {type.Name}");
                }

                injectableField.SetValue(injectable, resolved);
                Debug.Log($"Field Injected {fieldType.Name} into {type.Name}");
            }

            var injectableMethods = type.GetMethods(BINDING_FLAGS)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();

                var resolvedInstances = requiredParameters
                    .Select(Resolve)
                    .ToArray();

                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception($"Failed to resolve parameters for {injectableMethod.Name} in {type.Name}");
                }

                injectableMethod.Invoke(injectable, resolvedInstances);
                Debug.Log($"Method Injected {string.Join(", ", requiredParameters.Select(p => p.Name))} into {type.Name}");
            }
        }

        object Resolve(Type type)
        {
            registry.TryGetValue(type, out var instance);
            return instance;
        }

        static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(BINDING_FLAGS);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(BINDING_FLAGS);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute)))
                    continue;

                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null)
                {
                    registry.Add(returnType, providedInstance);
                    Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}");
                }
                else

                {
                    throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}");
                }
            }
        }

        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }

}