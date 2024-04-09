using System;

namespace BTG.Utilities.DI
{
    /// <summary>
    /// Attributes to mark fields and methods that should be injected by the DI system
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute to mark methods that provide dependencies to the DI system
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class  ProvideAttribute : Attribute
    {
    }
}