using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Caching;
using Cash.Core.Exceptions;

namespace Cash.Core
{
    public static class CashContext
    {
        private static IDictionary<Type, LambdaExpression> CacheKeyProviders = new Dictionary<Type, LambdaExpression>();

        public static ObjectCache CacheProvider { get; private set; }

        public static void ClearCacheProviders()
        {
            CacheKeyProviders = new Dictionary<Type, LambdaExpression>();
        }

        public static void SetCacheProvider(ObjectCache objectCache)
        {
            CacheProvider = objectCache;
        }
        
        public static void AddProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern)
        {
            var targetType = typeof (TEntity);

            if (CacheKeyProviders.ContainsKey(targetType))
            {
                throw new DuplicateCacheProviderRegistrationException(targetType);
            }

            // using this for inspiration: http://stackoverflow.com/questions/16678057/list-of-expressionfunct-tproperty
            CacheKeyProviders.Add(targetType, registrationPattern);
        }

        public static Expression<Func<TEntity, string>> GetProvider<TEntity>()
        {
            var targetType = typeof (TEntity);

            if (CacheKeyProviders.ContainsKey(targetType))
            {
                var provider = CacheKeyProviders[targetType];
                return (Expression<Func<TEntity, string>>)provider;
            }

            return null;
        }
    }
}
