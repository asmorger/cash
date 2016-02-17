using Autofac;
using Cash.Core.Services;
using System.Runtime.Caching;

namespace Cash.Core.Autofac
{
    public static class Cash
    {
        public static void RegisterCacheInfrastructure(ContainerBuilder builder, ObjectCache cacheProvider)
        {
            builder.Register(x => cacheProvider).As<ObjectCache>().SingleInstance();
            builder.RegisterType<CacheKeyGenerationService>().As<ICacheKeyGenerationService>().SingleInstance();
            builder.RegisterType<CacheKeyRegistrationService>().As<ICacheKeyRegistrationService>().SingleInstance();
        }
    }
}
