// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Cash.Core.Providers;
using Cash.Core.Providers.Base;

namespace Cash.Core.Services
{
    public class CacheKeyGenerationService : ICacheKeyGenerationService
    {
        private readonly ICacheKeyRegistrationService _cacheKeyRegistrationService;

        private readonly IOrderedEnumerable<ICacheKeyProvider> _cacheKeyProviders;

        private const string ArgumentNameValueDelimiter = "::";

        private const string IndividualArgumentDelimiter = "||";

        public CacheKeyGenerationService(ICacheKeyRegistrationService cacheKeyRegistrationService)
        {
            _cacheKeyRegistrationService = cacheKeyRegistrationService;

            _cacheKeyProviders = new List<ICacheKeyProvider>
                                        { new NullCacheKeyProvider(),
                                          new EumCacheKeyProvider(),
                                          new PrimitiveTypeCacheKeyProvider()
                                        }.OrderBy(x => (int)x.ExecutionOrder);
        }

        private string GetCacheKeyForArgument(object argument)
        {
            var provider = _cacheKeyProviders.FirstOrDefault(x => x.IsValid(argument));

            if (provider != null)
            {
                var result = provider.GetKey(argument);
                return result;
            }

            try
            {
                var type = argument.GetType();

                var getCacheKeyMethod = this.GetType()
                    .GetMethod(nameof(GetCacheKey), BindingFlags.NonPublic | BindingFlags.Instance);
                var typedGetCacheKeyMethod = getCacheKeyMethod.MakeGenericMethod(type);

                var cacheKey = (string)typedGetCacheKeyMethod.Invoke(this, new[] { argument });
                return cacheKey;
            }
            catch (TargetInvocationException ex)
            {
                // unwrap the reflection exception and re-throw the inner exception
                throw ex.InnerException;
            }
        }

        protected string GetCacheKey<TEntity>(TEntity item)
        {
            var cacheKeyProvider = _cacheKeyRegistrationService.GetTypedCacheKeyProvider<TEntity>();
            var typeName = typeof(TEntity).Name;

            var cacheKey = cacheKeyProvider(item);

            var output = $"{typeName}{ArgumentNameValueDelimiter}{cacheKey}";
            return output;
        }

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

        public string GetCacheKey(MethodInfo method, object[] arguments)
        {
            var methodCacheKey = GetMethodCacheKey(method);
            var argumentsCacheKey = GetArgumentsCacheKey(arguments);

            var output = $"{methodCacheKey}({argumentsCacheKey})";
            return output;
        }
    }
}