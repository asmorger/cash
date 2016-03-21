using Cash.Autofac.Sample.Web.Models;
using Cash.Core.Services;
using Cash.Sample.Shared.Models;

namespace Cash.Autofac.Sample.Web.App_Start
{
    public static class CashConfig
    {
        public static ICacheKeyRegistrationService RegisterCacheKeys()
        {
            var registrationService = new CacheKeyRegistrationService();

            registrationService.RegisterCacheKeyFormatter<UserModel>(x => $"{x.Id}");

            return registrationService;
        }
    }
}
