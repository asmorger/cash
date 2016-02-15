// // Copyright (c) Andrew Morger. All rights reserved.
// // Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

namespace Cash.Core.Services
{
    /// <summary>
    ///     Interface for providing abstractions to the underlying caching implementations
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        ///     Adds the item to the cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="item">The item to be cached.</param>
        /// <returns><c>true</c> if the item was successfully inserted into the cache, <c>false</c> otherwise.</returns>
        bool AddItem(string cacheKey, object item);

        /// <summary>
        ///     Gets the item from the cache
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>The object that was previously cached.</returns>
        object GetItem(string cacheKey);
    }
}