// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Cash.Core.Exceptions;

namespace Cash.Core.Services
{
    public interface ICacheKeyRegistry
    {
        /// <summary>
        ///     Adds the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the registration entity.</typeparam>
        /// <param name="registrationPattern">The registration pattern.</param>
        void Register<TEntity>(Expression<Func<TEntity, string>> registrationPattern);

        /// <summary>
        ///     Gets the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.String&gt;&gt;.</returns>
        Func<TEntity, string> Get<TEntity>();

        /// <summary>
        ///     Determines whether a typed cache key provider has been registered for a given type.
        /// </summary>
        /// <param name="type">The type to be checked</param>
        /// <returns><c>true</c> if [is provider registered] [the specified type]; otherwise, <c>false</c>.</returns>
        bool HasRegistration(Type type);
    }
    
    /// <summary>
    ///     Class CacheKeyRegistrationService.
    /// </summary>
    public class CacheKeyRegistry : ICacheKeyRegistry
    {
        private readonly IDictionary<Type, LambdaExpression> _cacheKeyFormatters = new Dictionary<Type, LambdaExpression>();

        /// <summary>
        ///     Adds the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the registration entity.</typeparam>
        /// <param name="registrationPattern">The registration pattern.</param>
        /// <exception cref="DuplicateCacheFormatterRegistrationException"></exception>
        public void Register<TEntity>(Expression<Func<TEntity, string>> registrationPattern)
        {
            var targetType = typeof(TEntity);

            if (_cacheKeyFormatters.ContainsKey(targetType))
            {
                throw new DuplicateCacheFormatterRegistrationException(targetType);
            }

            // using this for inspiration: http://stackoverflow.com/questions/16678057/list-of-expressionfunct-tproperty
            _cacheKeyFormatters.Add(targetType, registrationPattern);
        }

        /// <summary>
        ///     Gets the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.String&gt;&gt;.</returns>
        public Func<TEntity, string> Get<TEntity>()
        {
            var targetType = typeof(TEntity);

            if (_cacheKeyFormatters.ContainsKey(targetType))
            {
                var provider = _cacheKeyFormatters[targetType];
                var expression = (Expression<Func<TEntity, string>>)provider;
                return expression.Compile();
            }

            throw new UnregisteredCacheTypeException(typeof(TEntity));
        }

        public bool HasRegistration(Type type)
        {
            var output = _cacheKeyFormatters.ContainsKey(type);
            return output;
        }
    }
}