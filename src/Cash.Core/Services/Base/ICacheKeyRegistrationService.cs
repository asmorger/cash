using System;
using System.Linq.Expressions;

namespace Cash.Core.Services
{
    public interface ICacheKeyRegistrationService
    {
        /// <summary>
        /// Adds the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the registration entity.</typeparam>
        /// <param name="registrationPattern">The registration pattern.</param>
        void AddTypedCacheKeyProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern);

        /// <summary>
        /// Gets the typed cache key provider.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>Expression&lt;Func&lt;TEntity, System.String&gt;&gt;.</returns>
        Expression<Func<TEntity, string>> GetTypedCacheKeyProvider<TEntity>();

        /// <summary>
        /// Clears the cache key providers.
        /// </summary>
        void ClearCacheKeyProviders();
    }
}
