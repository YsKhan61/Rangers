using System;
using System.Collections.Generic;
using System.Reflection;

namespace BTG.Utilities
{
    public static class PredefinedAssemblyUtil
    {
        enum AssemblyType
        {
            Assembly_BTG_Utilities,
            Assembly_BTG_Enemy,
            Assembly_BTG_Player,
            Assembly_BTG_Bootstrap,
            Assembly_BTG_DIExample,
            Assembly_BTG_AudioSystem,
        }

        /// <summary>
        /// Gets a list of types from the assemblies that have the specified attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="types"></param>
        public static List<Type> GetTypesWithAttribute<T>()
        {
            Dictionary<AssemblyType, Type[]> assemblyTypes = new();
            List<Type> types = new();
            FilterAssemblyTypes(assemblyTypes, types);

            // AddTypesFromAssemblyUsingAttributes(assemblyTypes[AssemblyType.Assembly_BTG_DIExample], types, typeof(T));
            return types;
        }


        /// <summary>
        /// Gets a list of types from the assemblies that implement the specified interface type.
        /// </summary>
        /// <param name="interfaceType">The interface type to filter the types.</param>
        /// <returns>A list of types that implement the specified interface type.</returns>
        public static List<Type> GetTypes(Type interfaceType)
        {
            Dictionary<AssemblyType, Type[]> assemblyTypes = new();
            List<Type> types = new();
            FilterAssemblyTypes(assemblyTypes, types);

            AddTypesFromAssemblyUsingInterfaceType(assemblyTypes[AssemblyType.Assembly_BTG_Utilities], types, interfaceType);
            AddTypesFromAssemblyUsingInterfaceType(assemblyTypes[AssemblyType.Assembly_BTG_Enemy], types, interfaceType);
            AddTypesFromAssemblyUsingInterfaceType(assemblyTypes[AssemblyType.Assembly_BTG_Player], types, interfaceType);
            AddTypesFromAssemblyUsingInterfaceType(assemblyTypes[AssemblyType.Assembly_BTG_Bootstrap], types, interfaceType);
            // AddTypesFromAssemblyUsingInterfaceType(assemblyTypes[AssemblyType.Assembly_BTG_DIExample], types, interfaceType);
            AddTypesFromAssemblyUsingInterfaceType(assemblyTypes[AssemblyType.Assembly_BTG_AudioSystem], types, interfaceType);

            return types;
        }

        /// <summary>
        /// Gets the assembly type based on the assembly name.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <returns>The corresponding AssemblyType enum value if found, otherwise null.</returns>
        static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "BTG.Utilities" => AssemblyType.Assembly_BTG_Utilities,
                "BTG.Enemy" => AssemblyType.Assembly_BTG_Enemy,
                "BTG.Player" => AssemblyType.Assembly_BTG_Player,
                "BTG.Bootstrap" => AssemblyType.Assembly_BTG_Bootstrap,
                "BTG.DIExample" => AssemblyType.Assembly_BTG_DIExample,
                "BTG.AudioSystem" => AssemblyType.Assembly_BTG_AudioSystem,
                _ => null
            };
        }

        /// <summary>
        /// Filters the types from the assemblies based on the assembly type.
        /// Stores the types in the types collection.
        /// </summary>
        /// <param name="assemblyTypes"></param>
        /// <param name="types"></param>
        static void FilterAssemblyTypes(Dictionary<AssemblyType, Type[]> assemblyTypes, List<Type> types)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                AssemblyType? assemblyType = GetAssemblyType(assembly.GetName().Name);
                if (assemblyType != null)
                {
                    assemblyTypes.Add(assemblyType.Value, assembly.GetTypes());
                }
            }
        }

        /// <summary>
        /// Adds types from the specified assembly that implement the given interface type to the collection.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve types from.</param>
        /// <param name="types">The collection to add the types to.</param>
        /// <param name="interfaceType">The interface type to filter the types.</param>
        static void AddTypesFromAssemblyUsingInterfaceType(Type[] assembly, ICollection<Type> types, Type interfaceType)
        {
            if (assembly == null) return;

            foreach (Type type in assembly)
            {
                if (type != interfaceType && interfaceType.IsAssignableFrom(type))
                {
                    types.Add(type);
                }
            }
        }

        // Create a method that add types from the specified assembly that have the specified attribute to the collection.
        static void AddTypesFromAssemblyUsingAttributes(Type[] assembly, ICollection<Type> types, Type attributeType)
        {
            if (assembly == null) return;

            foreach (Type type in assembly)
            {
                // Get the attribute defined as AtributeTarget.Class, if found, check if it matches attributeType
                object[] attributes = type.GetCustomAttributes(attributeType, false);
                if (attributes.Length > 0)
                {
                    types.Add(type);
                }
            }
        }   
    }
}
