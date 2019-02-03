// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Caching.Memory;

namespace Cash.Core
{
    /// <summary>
    /// Represents the final configuration of an item to be cached.
    /// </summary>
    public interface ICacheConfiguration
    {
        /// <summary>
        /// The cache key to be used.
        /// </summary>
        string Key { get; }
        
        /// <summary>
        /// The item to be cached.
        /// </summary>
        object Item { get; }
        
        /// <summary>
        /// The options that control the caching behavior.
        /// </summary>
        MemoryCacheEntryOptions Options { get; }
    }
    
    /// <inheritdoc cref="ICacheConfiguration"/>
    public class CashConfiguration : ICacheConfiguration
    {
        public CashConfiguration(CacheAttribute attribute, object item, string key)
        {
            Options = attribute.GetCacheEntryOptions();
            Item = item;
            Key = key;
        }

        public string Key { get; }
        public object Item { get; }
        public MemoryCacheEntryOptions Options { get; }
    }
}