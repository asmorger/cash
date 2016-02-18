using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Cash.Core.Attributes;
using Cash.Core.Services;

namespace Cash.Core.Interceptors
{
    public abstract class BaseCachingInterceptor
    {
        protected readonly ObjectCache _cache;

        protected readonly ICacheKeyGenerationService _cacheKeyGenerationService;

        protected BaseCachingInterceptor(ObjectCache cache, ICacheKeyGenerationService cacheKeyGenerationService)
        {
            _cache = cache;
            _cacheKeyGenerationService = cacheKeyGenerationService;
        }

        protected abstract MethodInfo GetMethodInfoFromInterceptor();

        protected abstract object[] GetArgumentsFromInterceptor();

        protected abstract void SetIntercetporReturnValue(object value);

        protected abstract void InterceptorProceed();

        protected abstract object GetInterceptorReturnValue();

        public CacheAttribute GetCacheAttribute(MethodInfo method)
        {
            var cacheAttribute = method.GetCustomAttribute(typeof(CacheAttribute));

            if (cacheAttribute != null)
            {
                var typedAttribute = (CacheAttribute)cacheAttribute;
                return typedAttribute;
            }

            return null;
        }

        public CacheItem GetCacheItem(string key, object value)
        {
            var output = new CacheItem(key, value);
            return output;
        }

        protected void ExecuteInterceptionLogic()
        {
            var method = GetMethodInfoFromInterceptor();

            var cacheAttribute = GetCacheAttribute(method);

            // if the attribute doesn't exist on the method, then continue and exit
            if (cacheAttribute == null)
            {
                InterceptorProceed();
                return;
            }

            var arguments = GetArgumentsFromInterceptor();

            // get the cache key for the method and it's parameters
            var methodCacheKey = _cacheKeyGenerationService.GetCacheKey(method, arguments);


            // check to see if the cached item exists.  If so, retrieve it from the cache and return it
            if (_cache.Contains(methodCacheKey))
            {
                var result = _cache.Get(methodCacheKey);
                SetIntercetporReturnValue(result);
                return;
            }

            // the item is not cached - now go execute the method
            InterceptorProceed();

            var returnValue = GetInterceptorReturnValue();

            // cache the resulting output
            var cacheItem = GetCacheItem(methodCacheKey, returnValue);
            _cache.Set(cacheItem, new CacheItemPolicy());
        }
    }
}
