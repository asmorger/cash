// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

namespace Cash.Core.Interceptors
{
    public abstract class BaseCachingInterceptor
    {
        protected readonly IMemoryCache _cache;

        protected readonly ICacheKeyGenerator CacheKeyGenerator;

        protected BaseCachingInterceptor(IMemoryCache cache, ICacheKeyGenerator cacheKeyGenerator)
        {
            _cache = cache;
            CacheKeyGenerator = cacheKeyGenerator;
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
            var methodCacheKey = CacheKeyGenerator.Generate(method, arguments);

            WriteDebugMessage($"Cache Key generated: {methodCacheKey}");

            // check to see if the cached item exists.  If so, retrieve it from the cache and return it
            if (_cache.TryGetValue(methodCacheKey, out var cachedEntry))
            {
                SetIntercetporReturnValue(cachedEntry);

                WriteDebugMessage($"Cached method results being returned: {methodCacheKey}");

                return;
            }

            // the item is not cached - now go execute the method
            InterceptorProceed();

            var returnValue = GetInterceptorReturnValue();
            
            var cacheConfiguration = new CashConfiguration(cacheAttribute, returnValue, methodCacheKey);
            
            
            SetCache(cacheConfiguration);

            WriteDebugMessage($"Results cached for key: {methodCacheKey}");
        }

        private void SetCache(ICacheConfiguration config) => _cache.Set(config.Key, config.Item, config.Options);

        private void WriteDebugMessage(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
