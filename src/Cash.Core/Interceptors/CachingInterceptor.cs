using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Cash.Core.Attributes;
using Cash.Core.Services;

using Castle.DynamicProxy;

namespace Cash.Core.Interceptors
{
    public class CachingInterceptor : IInterceptor
    {
        private readonly ObjectCache _cache;
        private readonly ICacheKeyGenerationService _cacheKeyGenerationService;

        public CachingInterceptor(ObjectCache cache, ICacheKeyGenerationService cacheKeyGenerationService)
        {
            _cache = cache;
            _cacheKeyGenerationService = cacheKeyGenerationService;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.GetConcreteMethod();

            var cacheAttribute = GetCacheAttribute(method);

            // if the attribute doesn't exist on the method, then continue and exit
            if (cacheAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            // get the cache key for the method and it's parameters
            var methodCacheKey = _cacheKeyGenerationService.GetMethodCacheKey(method);

            // check to see if the cached item exists.  If so, retrieve it from the cache and return it
            if (_cache.Contains(methodCacheKey))
            {
                var result = _cache.Get(methodCacheKey);
                invocation.ReturnValue = result;
                return;
            }

            // the item is not cached - now go execute the method
            invocation.Proceed();

            // cache the resulting output
            var cacheItem = GetCacheItem(methodCacheKey, invocation.ReturnValue);
            _cache.Set(cacheItem, new CacheItemPolicy());

        }

        public CacheAttribute GetCacheAttribute(MethodInfo method)
        {
            var cacheAttribute = method.GetCustomAttribute(typeof (CacheAttribute));

            if (cacheAttribute != null)
            {
                var typedAttribute = (CacheAttribute) cacheAttribute;
                return typedAttribute;
            }

            return null;
        }

        public CacheItem GetCacheItem(string key, object value)
        {
            var output = new CacheItem(key, value);
            return output;
        }
    }
}
