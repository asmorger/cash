using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Cash.Core.Exceptions;

namespace Cash.Core.Services
{
    /// <summary>
    /// Class CacheKeyRegistrationService.
    /// </summary>
    public class CacheKeyRegistrationService : ICacheKeyRegistrationService
    {
        private readonly IDictionary<Type, LambdaExpression> _cacheKeyProviders = new Dictionary<Type, LambdaExpression>();

        /// <summary>
        /// Adds the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the registration entity.</typeparam>
        /// <param name="registrationPattern">The registration pattern.</param>
        /// <exception cref="DuplicateCacheProviderRegistrationException"></exception>
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

        /// <summary>
        /// Gets the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.String&gt;&gt;.</returns>
        public Func<TEntity, string> GetTypedCacheKeyProvider<TEntity>()
        {
            var targetType = typeof(TEntity);

            if (_cacheKeyProviders.ContainsKey(targetType))
            {
                var provider = _cacheKeyProviders[targetType];
                var expression = (Expression<Func<TEntity, string>>)provider;
                return expression.Compile();
            }

            return null;
        }

        /// <summary>
        /// Clears the cache key providers.
        /// </summary>
        public void ClearCacheKeyProviders()
        {
            _cacheKeyProviders.Clear();
        }
    }
}
