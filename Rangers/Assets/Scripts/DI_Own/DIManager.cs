using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using System.Linq;
using BTG.Utilities;

namespace BTG.DI_Own
{
    [DefaultExecutionOrder(-1000)]
    public class DIManager : SingletonPersistent<DIManager>
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            RegisterDependencies();

            InjectDependencies();
        }

        /// <summary>
        /// Register all the dependency providers.
        /// </summary>
        void RegisterDependencies()
        {
            List<Type> typesToRegister = PredefinedAssemblyUtil.GetTypes(typeof(ISelfDependencyRegister));

            foreach (Type type in typesToRegister)
            {
                RegisterType(type);
            }

            var providers = FindMonoBehaviours().OfType<IDependencyProviderForOthers>();
            foreach (var provider in providers)
            {
                RegisterFromProvider(provider);
            }

            typesToRegister = PredefinedAssemblyUtil.GetTypesWithAttribute<RegisterAttribute>();
            foreach (Type type in typesToRegister)
            {
                RegisterType(type);
            }
        }

        /// <summary>
        /// Inject dependencies into all the dependency injectable types.
        /// </summary>
        void InjectDependencies()
        {
            List<Type> dependencyInjectableTypes = PredefinedAssemblyUtil.GetTypes(typeof(IDependencyInjector));

            foreach (Type injectableType in dependencyInjectableTypes)
            {
                Inject(injectableType);
            }

            var injectables = FindMonoBehaviours().OfType<IDependencyInjector>();
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }

            List<Type> typesToInject = PredefinedAssemblyUtil.GetTypesWithAttribute<InjectorAttribute>();
            foreach (Type type in typesToInject)
            {
                Inject(type);
            }
        }

        /// <summary>
        /// Register a type and provide an instance of the object of the specified type by creating it.
        /// </summary>
        /// <param name="type">The type of the object to be registered</param>
        /// <returns>The instance of the registered object</returns>
        public object RegisterType(Type type)
        {
            if (registry.ContainsKey(type))
            {
                Debug.LogWarning($"Object of type {type.Name} is already registered");
                return registry[type];
            }

            object instance;

            // Check if the type is a MonoBehaviour
            if (typeof(MonoBehaviour).IsAssignableFrom(type))
            {
                instance = FindObjectOfType(type) ?? gameObject.AddComponent(type);
                registry.Add(type, instance);
                return instance;
            }

            instance = Activator.CreateInstance(type);
            registry.Add(type, instance);
            return instance;
        }

        /// <summary>
        /// Registers an instance of the object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to be registered</typeparam>
        /// <param name="instance">The instance to be registered</param>
        public void RegisterInstance<T>(T instance)
        {
            var type = typeof(T);
            if (registry.ContainsKey(type))
            {
                Debug.LogWarning($"Object of type {type.Name} is already registered");
                return;
            }

            registry.Add(type, instance);
        }

        /// <summary>
        /// Inject dependencies into the specified injectable object.
        /// </summary>
        /// <param name="injectable">The object to inject dependencies into</param>
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

        /// <summary>
        /// Inject dependencies into the specified type.
        /// </summary>
        /// <param name="type">The type to inject dependencies into</param>
        public void Inject(Type type)
        {
            // Return if the type is a monobehaviour
            if (typeof(MonoBehaviour).IsAssignableFrom(type))
            {
                return;
            }

            var injectable = Resolve(type);
            if (injectable == null)
            {
                throw new Exception($"Failed to resolve {type.Name}. Make sure this type is provided before injecting dependencies in it.");
            }

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

        /// <summary>
        /// Resolve an instance of the specified type from the registry.
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <returns>The resolved instance</returns>
        public object Resolve(Type type)
        {
            registry.TryGetValue(type, out var instance);
            return instance;
        }

        /// <summary>
        /// Register a provider object and provide instances of the specified types using the provider's methods.
        /// </summary>
        /// <param name="provider">The provider object</param>
        void RegisterFromProvider(object provider)
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

        /// <summary>
        /// Find all MonoBehaviours in the scene.
        /// </summary>
        /// <returns>An array of MonoBehaviours</returns>
        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}