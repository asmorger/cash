﻿using System;
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
        private readonly ICacheKeyGenerationService _cacheKeyGenerationService = new CacheKeyGenerationService();

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
            if (CashContext.Instance.CacheBackingStore.Contains(methodCacheKey))
            {
                var result = CashContext.Instance.CacheBackingStore.Get(methodCacheKey);
                invocation.ReturnValue = result;
                return;
            }

            // the item is not cached - now go execute the method
            invocation.Proceed();

            // cache the resulting output
            var cacheItem = GetCacheItem(methodCacheKey, invocation.ReturnValue);
            CashContext.Instance.CacheBackingStore.Set(cacheItem, new CacheItemPolicy());

        }

        private CacheAttribute GetCacheAttribute(MethodInfo method)
        {
            var cacheAttribute = method.GetCustomAttribute(typeof (CacheAttribute));

            if (cacheAttribute != null)
            {
                var typedAttribute = (CacheAttribute) cacheAttribute;
                return typedAttribute;
            }

            return null;
        }

        private CacheItem GetCacheItem(string key, object value)
        {
            var output = new CacheItem(key, value);
            return output;
        }
    }
}
