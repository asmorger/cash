using System.Runtime.Caching;
using Autofac;

using Cash.Core.Interceptors;
using Cash.Core.Services;

namespace Cash.Autofac
{
    public static class Cash
    {
        /// <summary>
        /// Adds the Cash-specific registrations to the Autofac container builder.
        /// </summary>
        /// <param name="builder">The Autofac <see cref="ContainerBuilder"/></param>
        /// <param name="cacheProvider">The <see cref="ObjectCache"/> instance to use as a cache provider.</param>
        /// <param name="registrationService">The <see cref="ICacheKeyRegistrationService"/> that contains the registrations for this solution. </param>
        public static void RegisterCacheInfrastructure(ContainerBuilder builder, ObjectCache cacheProvider, ICacheKeyRegistrationService registrationService)
        {
            builder.Register(x => cacheProvider).As<ObjectCache>().SingleInstance();
            builder.RegisterType<CacheKeyGenerationService>().As<ICacheKeyGenerationService>().SingleInstance();
            builder.Register(x => registrationService).As<ICacheKeyRegistrationService>().SingleInstance();

            builder.Register(c => new CachingInterceptor(c.Resolve<ObjectCache>(), c.Resolve<ICacheKeyGenerationService>())).InstancePerDependency();
        }
    }
}
