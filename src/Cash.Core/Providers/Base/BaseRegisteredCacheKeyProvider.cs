using System;
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
            // since we override GetKey(parameter) we don't need this method
            throw new System.NotImplementedException();
        }

        public override string GetValueRepresentation(object parameter)
        {
            // since we override GetKey(parameter) we don't need this method
            throw new System.NotImplementedException();
        }

        public override string GetKey(object parameter)
        {
            try
            {
                var type = parameter.GetType();

                var getCacheKeyMethod = this.GetType().GetMethod(nameof(GetCacheKey), BindingFlags.NonPublic | BindingFlags.Instance);
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

        // this needs to be public so that the reflection in the GetKey() method works properly in this and any derived classes
        protected string GetCacheKey<TEntity>(TEntity item)
        {
            var cacheKeyProvider = CacheKeyRegistrationService.GetTypedCacheKeyProvider<TEntity>();
            var typeName = typeof(TEntity).Name;

            var cacheKey = cacheKeyProvider(item);

            var output = $"{typeName}{ArgumentNameValueDelimiter}{cacheKey}";
            return output;
        }
    }
}
