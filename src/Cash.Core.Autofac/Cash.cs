using Autofac;
using Cash.Core.Services;
using System.Runtime.Caching;

namespace Cash.Core.Autofac
{
    public static class Cash
    {
        /// <summary>
        /// Adds the Cash-specific registrations to the Autofac container builder.
        /// </summary>
        /// <param name="builder">The Autofac <see cref="ContainerBuilder"/></param>
        /// <param name="cacheProvider">The <see cref="ObjectCache"/> instance to use as a cache provider.</param>
        public static void RegisterCacheInfrastructure(ContainerBuilder builder, ObjectCache cacheProvider)
        {
            builder.Register(x => cacheProvider).As<ObjectCache>().SingleInstance();
            builder.RegisterType<CacheKeyGenerationService>().As<ICacheKeyGenerationService>().SingleInstance();
            builder.RegisterType<CacheKeyRegistrationService>().As<ICacheKeyRegistrationService>().SingleInstance();
        }
    }
}
