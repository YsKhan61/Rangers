using System;
using System.Collections.Generic;
using System.Reflection;

namespace BTG.Utilities
{
    public static class PredefinedAssemblyUtil
    {
        enum AssemblyType
        {
            Assembly_BTG_Utilities
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
                _ => null
            };
        }

        /// <summary>
        /// Adds types from the specified assembly that implement the given interface type to the collection.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve types from.</param>
        /// <param name="types">The collection to add the types to.</param>
        /// <param name="interfaceType">The interface type to filter the types.</param>
        static void AddTypesFromAssembly(Type[] assembly, ICollection<Type> types, Type interfaceType)
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

        /// <summary>
        /// Gets a list of types from the assemblies that implement the specified interface type.
        /// </summary>
        /// <param name="interfaceType">The interface type to filter the types.</param>
        /// <returns>A list of types that implement the specified interface type.</returns>
        public static List<Type> GetTypes(Type interfaceType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<AssemblyType, Type[]> assemblyTypes = new();
            List<Type> types = new();
            foreach (Assembly assembly in assemblies)
            {
                AssemblyType? assemblyType = GetAssemblyType(assembly.GetName().Name);
                if (assemblyType != null)
                {
                    assemblyTypes.Add(assemblyType.Value, assembly.GetTypes());
                }
            }

            AddTypesFromAssembly(assemblyTypes[AssemblyType.Assembly_BTG_Utilities], types, interfaceType);

            return types;
        }
    }
}
