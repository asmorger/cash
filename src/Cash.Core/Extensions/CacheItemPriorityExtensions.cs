// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Cash.Core.Extensions
{
    public static class CacheItemPriorityExtensions
    {
        public static System.Runtime.Caching.CacheItemPriority Convert(this CacheItemPriority priority)
        {
            switch (priority)
            {
                case CacheItemPriority.NeverRemove:
                    return System.Runtime.Caching.CacheItemPriority.NotRemovable;
                default:
                    return System.Runtime.Caching.CacheItemPriority.Default;
            }
        }
    }
}
