// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Runtime.Caching;
using Cash.Core.Extensions;

namespace Cash.Core
{
    /// <summary>
    ///     Attribute used to indicate that a method should be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
        /// <summary>
        ///     Gets the priority at which something should be cached.
        /// </summary>
        /// <value>The priority that should be used</value>
        public CacheItemPriority Priority { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CacheAttribute" /> class.
        /// </summary>
        /// <param name="priority">The <see cref="CacheItemPriority" /> priority that should be applied to this method's output.</param>
        public CacheAttribute(CacheItemPriority priority = CacheItemPriority.Normal)
        {
            Priority = priority;
        }

        /// <summary>
        ///     Gets the configured CacheItemPolicy based upon the properties of the attribute
        /// </summary>
        /// <returns>The <see cref="CacheItemPolicy"/> that will be applied to the cache item.</returns>
        public CacheItemPolicy GetCacheItemPolicy()
        {
            var policy = new CacheItemPolicy { Priority = Priority.Convert() };
            return policy;
        }
    }
}