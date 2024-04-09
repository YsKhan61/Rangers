using System;

namespace BTG.Utilities.DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class  ProvideAttribute : Attribute
    {
    }
}