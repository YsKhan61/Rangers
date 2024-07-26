namespace BTG.DI_Own
{
    /// <summary>
    /// An empty interface to mark a class as a dependency provider for other class's dependencies (scriptable objects)
    /// Note - this class does not register itself, it just provides instances of other classes to the DIManager.
    /// </summary>
    public interface IDependencyProviderForOthers { }
}