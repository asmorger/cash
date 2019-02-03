// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Caching.Memory;

namespace Cash.Core
{
    public interface ICacheConfiguration
    {
        string Key { get; }
        object Item { get; }
        MemoryCacheEntryOptions Options { get; }
    }
    
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