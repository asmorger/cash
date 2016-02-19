// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Cash.Core.Services
{
    public class CacheKeyGenerationService : ICacheKeyGenerationService
    {
        private readonly ICacheKeyRegistrationService _cacheKeyRegistrationService;

        private const string ArgumentNameValueDelimiter = "::";

        private const string IndividualArgumentDelimiter = "||";

        public CacheKeyGenerationService(ICacheKeyRegistrationService cacheKeyRegistrationService)
        {
            _cacheKeyRegistrationService = cacheKeyRegistrationService;
        }

        private string GetCacheKeyForArgument(object argument)
        {
            var type = argument.GetType();

            if (type.IsPrimitive || type == typeof(string))
            {
                var primitiveCacheKey = GetPrimitiveCacheKey(argument, type);
                return primitiveCacheKey;
            }

            try
            {
                var getCacheKeyMethod = GetType()
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

        private string GetPrimitiveCacheKey(object argument, Type type)
        {
            var typeName = type.Name;

            var output = $"{typeName}{ArgumentNameValueDelimiter}{argument}";
            return output;
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