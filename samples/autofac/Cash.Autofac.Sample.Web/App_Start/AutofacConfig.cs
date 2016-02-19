using System.Runtime.Caching;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using Cash.Autofac.Extensions;
using Cash.Autofac.Sample.Web.Services;

namespace Cash.Autofac.Sample.Web.App_Start
{
    public static class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            Cash.RegisterCacheInfrastructure(builder, MemoryCache.Default);

            builder.RegisterType<RandomDataService>().As<IRandomDataService>().SingleInstance().WithDefaultCache();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
