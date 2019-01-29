// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Reflection;
using Cash.Core.Exceptions;
using Cash.Core.Providers;
using Cash.Core.Providers.Base;

namespace Cash.Core
{
    public interface ICacheKeyGenerator
    {
        string GenerateForArguments(object[] arguments);
        string Generate(MethodInfo method, object[] arguments);
    }
    
    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        private readonly ICacheKeyProvider[] _cacheKeyProviders;

        private const string IndividualArgumentDelimiter = "||";
        private const string ArgumentNameValueDelimiter = "::";

        public CacheKeyGenerator(ICacheKeyRegistry cacheKeyRegistry)
        {

            _cacheKeyProviders = new ICacheKeyProvider[]
                                        { new NullCacheKeyProvider(),
                                          new EumCacheKeyProvider(),
                                          new PrimitiveTypeCacheKeyProvider(),
                                          new UserRegisteredCacheKeyProvider(cacheKeyRegistry) 
                                        };
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
            ICacheKeyProvider provider = null;

            foreach (var cacheKeyProvider in _cacheKeyProviders)
            {
                if (cacheKeyProvider.IsValid(argument))
                {
                    provider = cacheKeyProvider;
                    break;
                }
            }
            
            if (provider != null)
            {
                var key = provider.GetTypeNameRepresentation(argument);
                var value = provider.GetValueRepresentation(argument);

                var output = $"{key}{ArgumentNameValueDelimiter}{value}";
                return output;
            }

            return string.Empty;
        }

        public string GenerateForArguments(object[] arguments)
        {
            if (arguments == null || !arguments.Any())
            {
                return "<no_arguments>";
            }

            var cacheKeys = arguments.Select(GetCacheKeyForArgument);
            var cacheKey = string.Join(IndividualArgumentDelimiter, cacheKeys);

            return cacheKey;
        }

        public string Generate(MethodInfo method, object[] arguments)
        {
            var type = method.DeclaringType;

            var className = $"{type.Namespace}.{type.Name}";
            var methodName = method.Name;
            var typeNames = method.ReturnType.GenericTypeArguments.Any()
                    ? string.Join(",", method.ReturnType.GenericTypeArguments.Select(x => x.Name))
                    : string.Empty;

            var keyFormat = string.IsNullOrEmpty(typeNames) ? "{0}.{1}" : "{0}.{1}<{2}>";
            var formattedKey = string.Format(keyFormat, className, methodName, typeNames);
            var argumentsCacheKey = GenerateForArguments(arguments);

            var output = string.Concat(formattedKey, $"({argumentsCacheKey})");
            return output;
        }
    }
}