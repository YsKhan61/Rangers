using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using System.Linq;

namespace BTG.Utilities.DI
{
    [DefaultExecutionOrder(-1000)]
    public class DIManager : SingletonPersistent<DIManager>
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        public override void Awake()
        {
            base.Awake();

            RegisterInstancesFromProviders();

            InjectDependencies();
        }

        void RegisterInstancesFromProviders()
        {
            List<Type> dependencyProviderTypes = PredefinedAssemblyUtil.GetTypes(typeof(IDependencyProvider));

            foreach (Type providerType in dependencyProviderTypes)
            {
                RegisterProvider(providerType);
            }

            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();
            foreach (var provider in providers)
            {
                RegisterProvider(provider);
            }
        }

        void InjectDependencies()
        {
            List<Type> dependencyInjectableTypes = PredefinedAssemblyUtil.GetTypes(typeof(IDependencyInjectable));

            foreach (Type injectableType in dependencyInjectableTypes)
            {
                var instance = Activator.CreateInstance(injectableType);
                Inject(instance);
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

        public void Inject(object injectable)
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

        public object Resolve(Type type)
        {
            registry.TryGetValue(type, out var instance);
            return instance;
        }

        void RegisterProvider(object provider)
        {
            var methods = provider.GetType().GetMethods(BINDING_FLAGS);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute)))
                    continue;

                var returnType = method.ReturnType;
                if (registry.ContainsKey(returnType))
                {
                    Debug.LogWarning($"Object of type {returnType.Name} is already registered");
                    return;
                }

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