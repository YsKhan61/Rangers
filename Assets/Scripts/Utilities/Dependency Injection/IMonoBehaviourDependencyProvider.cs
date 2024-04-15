namespace BTG.Utilities.DI
{
    /// <summary>
    /// An empty interface to mark a class as a dependency provider
    /// Any class that provides dependencies to the DI system must implement this interface
    /// </summary>
    public interface IMonoBehaviourDependencyProvider { }

    /// <summary>
    /// An empty interface to mark a class as a self dependency provider
    /// Any class that provides itself as a dependency to the DI system must implement this interface
    /// </summary>
    public interface ISelfDependencyProvider { }
}