using System.Reflection;

namespace Cash.Core.Services
{
    public interface ICacheKeyGenerationService
    {
        string GetMethodCacheKey(MethodInfo method);

        string GetArgumentsCacheKey(object[] arguments);
    }
}
