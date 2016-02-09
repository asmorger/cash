
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

            var cacheKeys = arguments.Select(a => GetCacheKeyForArgument(a, nameof(a)));
            var cacheKey = string.Join(IndividualArgumentDelimiter, cacheKeys);

            return cacheKey;
        }

        private string GetCacheKeyForArgument(object argument, string argumentName)
        {
            var type = argument.GetType();

            var getCacheKeyMethod = this.GetType().GetMethod("GetCacheKey", BindingFlags.Public | BindingFlags.Instance);
            var typedGetCacheKeyMethod = getCacheKeyMethod.MakeGenericMethod(type);

            var cacheKey = (string)typedGetCacheKeyMethod.Invoke(this, new[] { argument, argumentName });
            return cacheKey;
        }

        public string GetCacheKey<TEntity>(TEntity item, string argumentName)
        {
            var cacheKeyProvider = CashContext.Instance.RegistrationService.GetTypedCacheKeyProvider<TEntity>();

            if (cacheKeyProvider == null)
            {
                return $"{argumentName}{ArgumentNameValueDelimiter}NULL";
            }

            var cacheKey = cacheKeyProvider(item);

            var output = $"{argumentName}{ArgumentNameValueDelimiter}{cacheKey}";
            return output;
        }
    }
}
