using System.Runtime.Caching;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using Cash.Autofac.Extensions;
using Cash.Autofac.Sample.Web.Models;
using Cash.Autofac.Sample.Web.Services;
using Cash.Core.Services;

namespace Cash.Autofac.Sample.Web.App_Start
{
    public static class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var registrationService = GetCacheKeyRegistrations();
            Cash.RegisterCacheInfrastructure(builder, MemoryCache.Default, registrationService);

            builder.RegisterType<RandomDataService>().As<IRandomDataService>().SingleInstance().WithDefaultCache();
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance().WithDefaultCache();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static ICacheKeyRegistrationService GetCacheKeyRegistrations()
        {
            var registrationService = new CacheKeyRegistrationService();

            registrationService.AddTypedCacheKeyProvider<UserModel>(x => $"{x.Id}");

            return registrationService;
        }
    }
}
