// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Caching.Memory;

namespace Cash.Core
{
    /// <summary>
    ///     Attribute used to indicate that a method should be cached.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
        /// <summary>
        /// The priority at which something should be cached.
        /// </summary>
        public CacheItemPriority Priority { get; }

        /// <summary>
        /// The maximum duration that the item should be cached. 
        /// </summary>
        public double? Duration { get; }

        /// <summary>
        /// The time measurement by which the <see cref="Duration"/> is applied.
        /// </summary>
        public TimeMeasure Measure { get; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="CacheAttribute" /> class.
        /// </summary>
        /// <param name="priority">The <see cref="CacheItemPriority" /> priority that should be applied to this method's output.</param>
        public CacheAttribute(CacheItemPriority priority = CacheItemPriority.Normal)
        {
            Priority = priority;
        }
        
        /// <summary>
        ///  Initializes a new instance of the <see cref="CacheAttribute" /> class.
        /// </summary>
        public CacheAttribute(double duration, TimeMeasure measure)
        {
            Priority = CacheItemPriority.Normal;
            Duration = duration;
            Measure = measure;
        }

        /// <summary>
        /// Constructs a <see cref="MemoryCacheEntryOptions"/> from the properties of this class.
        /// </summary>
        public MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            // default to minutes
            TimeSpan ConvertDuration()
            {
                switch (Measure)
                {
                    case TimeMeasure.Seconds:
                        return TimeSpan.FromSeconds(Duration.Value);
                    case TimeMeasure.Hours:
                        return TimeSpan.FromHours(Duration.Value);
                    // ReSharper disable once RedundantCaseLabel
                    case TimeMeasure.Minutes:
                    default:
                        return TimeSpan.FromMinutes(Duration.Value);
                }
            }
            
            var options = new MemoryCacheEntryOptions
            {
                Priority = Priority
            };

            if (Duration.HasValue)
            {
                options.SlidingExpiration = ConvertDuration();
            }

            return options;
        }
    }

    /// <summary>
    /// Represents different increments of time measurement
    /// </summary>
    public enum TimeMeasure
    {
        Seconds,
        Minutes,
        Hours
    }
}