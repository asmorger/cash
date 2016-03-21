// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Reflection;

using Cash.Core.Services;

namespace Cash.Core.Providers.Base
{
    public abstract class BaseRegisteredCacheKeyProvider : BaseCacheKeyProvider
    {
        protected readonly ICacheKeyRegistrationService CacheKeyRegistrationService;

        protected BaseRegisteredCacheKeyProvider(ICacheKeyRegistrationService cacheKeyRegistrationService)
        {
            CacheKeyRegistrationService = cacheKeyRegistrationService;
        }

        public override string GetTypeNameRepresentation(object parameter)
        {
            var type = parameter.GetType();
            var key = type.Name;

            return key;
        }

        public override string GetValueRepresentation(object parameter)
        {
            try
            {
                var type = parameter.GetType();

                var getCacheKeyMethod = this.GetType().GetMethod(nameof(GetCacheKeyValueRepresentation), BindingFlags.NonPublic | BindingFlags.Instance);
                var typedGetCacheKeyMethod = getCacheKeyMethod.MakeGenericMethod(type);

                var cacheKey = (string)typedGetCacheKeyMethod.Invoke(this, new[] { parameter });
                return cacheKey;
            }
            catch (TargetInvocationException ex)
            {
                // unwrap the reflection exception and re-throw the inner exception
                throw ex.InnerException;
            }
        }

        protected string GetCacheKeyValueRepresentation<TEntity>(TEntity item)
        {
            var cacheKeyProvider = CacheKeyRegistrationService.GetCacheKeyFormatter<TEntity>();
            var value = cacheKeyProvider(item);

            return value;
        }
    }
}
