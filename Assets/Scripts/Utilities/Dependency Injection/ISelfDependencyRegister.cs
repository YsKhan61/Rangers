namespace BTG.Utilities.DI
{
    /// <summary>
    /// An empty interface to mark a class as a self dependency provider
    /// Any class that provides itself as a dependency to the DI system must implement this interface
    /// Also make sure that the AssemblyDefinition of the class is defined in PredefinedAssemblyUtil.cs
    /// either using Attributes or Interface type 
    /// [ AddTypesFromAssemblyUsingAttributes() or AddTypesFromAssemblyUsingInterfaceType() ]
    /// </summary>
    public interface ISelfDependencyRegister { }
}