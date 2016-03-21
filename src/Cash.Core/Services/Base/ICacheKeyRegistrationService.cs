// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;

namespace Cash.Core.Services
{
    public interface ICacheKeyRegistrationService
    {
        /// <summary>
        ///     Adds the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the registration entity.</typeparam>
        /// <param name="registrationPattern">The registration pattern.</param>
        void AddTypedCacheKeyProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern);

        /// <summary>
        ///     Gets the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.String&gt;&gt;.</returns>
        Func<TEntity, string> GetTypedCacheKeyProvider<TEntity>();

        /// <summary>
        ///     Determines whether a typed cache key provider has been registered for a given type.
        /// </summary>
        /// <param name="type">The type to be checked</param>
        /// <returns><c>true</c> if [is provider registered] [the specified type]; otherwise, <c>false</c>.</returns>
        bool IsProviderRegistered(Type type);
    }
}