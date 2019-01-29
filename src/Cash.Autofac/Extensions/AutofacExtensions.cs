// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Runtime.Caching;
using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Cash.Core;
using Cash.Core.Interceptors;

namespace Cash.Autofac.Extensions
{
    /// <summary>
    /// A class to house Autofac Extension Methods
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// Enables support for caching the method output based upon it's type, name, and arguments
        /// </summary>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> WithCaching<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            return registration.EnableInterfaceInterceptors().InterceptedBy(typeof(CachingInterceptor));
        }

        public static void AddCaching(this ContainerBuilder builder, ICacheKeyRegistry registry, ObjectCache cacheProvider = null)
        {
            var definedCacheProvider = cacheProvider ?? MemoryCache.Default;
            builder.Register(x => definedCacheProvider).As<ObjectCache>().SingleInstance();
            builder.RegisterType<CacheKeyGenerator>().As<ICacheKeyGenerator>().SingleInstance();
            builder.Register(x => registry).As<ICacheKeyRegistry>().SingleInstance();

            builder.Register(c => new CachingInterceptor(c.Resolve<ObjectCache>(), c.Resolve<ICacheKeyGenerator>())).InstancePerDependency();
        }
    }
}
