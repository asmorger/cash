using Cash.Autofac.Sample.Web.Models;
using Cash.Core.Services;

namespace Cash.Autofac.Sample.Web.App_Start
{
    public static class CashConfig
    {
        public static ICacheKeyRegistrationService RegisterCacheKeys()
        {
            var registrationService = new CacheKeyRegistrationService();

            registrationService.AddTypedCacheKeyProvider<UserModel>(x => $"{x.Id}");

            return registrationService;
        }
    }
}
