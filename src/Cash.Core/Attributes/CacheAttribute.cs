using System;

namespace Cash.Core.Attributes
{
    /// <summary>
    /// Attribute used to indicate that a method should be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
    }
}
