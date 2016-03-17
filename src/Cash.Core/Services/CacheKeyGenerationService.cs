// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Cash.Core.Exceptions;
using Cash.Core.Providers;
using Cash.Core.Providers.Base;

namespace Cash.Core.Services
{
    public class CacheKeyGenerationService : ICacheKeyGenerationService
    {
        private readonly IOrderedEnumerable<ICacheKeyProvider> _cacheKeyProviders;

        private const string IndividualArgumentDelimiter = "||";

        public CacheKeyGenerationService(ICacheKeyRegistrationService cacheKeyRegistrationService)
        {

            _cacheKeyProviders = new List<ICacheKeyProvider>
                                        { new NullCacheKeyProvider(),
                                          new EumCacheKeyProvider(),
                                          new PrimitiveTypeCacheKeyProvider(),
                                          new UserRegisteredCacheKeyProvider(cacheKeyRegistrationService) 
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

            var type = argument.GetType();
            throw new UnregisteredCacheTypeException(type);
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