
using System;
using System.Linq;
using System.Reflection;

namespace Cash.Core.Services
{
    public class CacheKeyGenerationService : ICacheKeyGenerationService
    {
        private const string ArgumentNameValueDelimiter = "::";

        private const string IndividualArgumentDelimiter = "||";

        public string GetMethodCacheKey(MethodInfo method)
        {
            var typeName = method.DeclaringType == null ? "<unknown>" : method.DeclaringType.FullName;

            var cacheKey = $"{typeName}.{method.Name}";
            return cacheKey;
        }

        public string GetArgumentsCacheKey(object[] arguments)
        {
            if (arguments == null || !arguments.Any())
            {
                return "<no_arguments>";
            }

            var cacheKeys = arguments.Select(GetCacheKeyForArgument);
            var cacheKey = string.Join(IndividualArgumentDelimiter, cacheKeys);

            return cacheKey;
        }

        private string GetCacheKeyForArgument(object argument)
        {
            var type = argument.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                var primitiveCacheKey = GetPrimitiveCacheKey(argument, type);
                return primitiveCacheKey;
            }

            var getCacheKeyMethod = this.GetType().GetMethod(nameof(GetCacheKey), BindingFlags.Public | BindingFlags.Instance);
            var typedGetCacheKeyMethod = getCacheKeyMethod.MakeGenericMethod(type);

            var cacheKey = (string)typedGetCacheKeyMethod.Invoke(this, new[] { argument });
            return cacheKey;
        }

        private string GetPrimitiveCacheKey(object argument, Type type)
        {
            var typeName = type.Name;

            var output = $"{typeName}{ArgumentNameValueDelimiter}{argument}";
            return output;
        }

        public string GetCacheKey<TEntity>(TEntity item)
        {
            var cacheKeyProvider = CashContext.Instance.RegistrationService.GetTypedCacheKeyProvider<TEntity>();
            var typeName = typeof (TEntity).Name;

            if (cacheKeyProvider == null)
            {
                return $"{typeName}{ArgumentNameValueDelimiter}NULL";
            }

            var cacheKey = cacheKeyProvider(item);

            var output = $"{typeName}{ArgumentNameValueDelimiter}{cacheKey}";
            return output;
        }
    }
}
