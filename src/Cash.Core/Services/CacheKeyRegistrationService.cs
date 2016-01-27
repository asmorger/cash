using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Cash.Core.Exceptions;

namespace Cash.Core.Services
{
    public class CacheKeyRegistrationService : ICacheKeyRegistrationService
    {
        private readonly IDictionary<Type, LambdaExpression> _cacheKeyProviders = new Dictionary<Type, LambdaExpression>();

        public void AddTypedCacheKeyProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern)
        {
            var targetType = typeof(TEntity);

            if (_cacheKeyProviders.ContainsKey(targetType))
            {
                throw new DuplicateCacheProviderRegistrationException(targetType);
            }

            // using this for inspiration: http://stackoverflow.com/questions/16678057/list-of-expressionfunct-tproperty
            _cacheKeyProviders.Add(targetType, registrationPattern);
        }

        public Expression<Func<TEntity, string>> GetTypedCacheKeyProvider<TEntity>()
        {
            var targetType = typeof(TEntity);

            if (_cacheKeyProviders.ContainsKey(targetType))
            {
                var provider = _cacheKeyProviders[targetType];
                return (Expression<Func<TEntity, string>>)provider;
            }

            return null;
        }

        public void ClearCacheKeyProviders()
        {
            _cacheKeyProviders.Clear();
        }
    }
}
