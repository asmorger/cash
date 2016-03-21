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
        private const string ArgumentNameValueDelimiter = "::";

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
            var value = GetValueRepresentationFromProvider(argument);

            if (string.IsNullOrEmpty(value))
            {
                var type = argument.GetType();
                throw new UnregisteredCacheTypeException(type);
            }

            return value;
        }

        private string GetValueRepresentationFromProvider(object argument)
        {
            var provider = _cacheKeyProviders.FirstOrDefault(x => x.IsValid(argument));

            if (provider != null)
            {
                var key = provider.GetTypeNameRepresentation(argument);
                var value = provider.GetValueRepresentation(argument);

                var output = $"{key}{ArgumentNameValueDelimiter}{value}";
                return output;
            }

            return string.Empty;
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
            var type = method.DeclaringType;

            var className = $"{type.Namespace}.{type.Name}";
            var methodName = method.Name;
            var typeNames = method.ReturnType.GenericTypeArguments.Any()
                    ? string.Join(",", method.ReturnType.GenericTypeArguments.Select(x => x.Name))
                    : string.Empty;

            var keyFormat = string.IsNullOrEmpty(typeNames) ? "{0}.{1}" : "{0}.{1}<{2}>";
            var output = string.Format(keyFormat, className, methodName, typeNames);
            var argumentsCacheKey = GetArgumentsCacheKey(arguments);

            if (arguments!= null && arguments.Any())
            {
                output = string.Concat(output, "||", argumentsCacheKey);
            }
            else
            {
                output = string.Concat(output, $"({argumentsCacheKey})");
            }

            return output;
        }
    }
}