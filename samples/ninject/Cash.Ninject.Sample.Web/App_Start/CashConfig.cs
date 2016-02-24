using Cash.Core.Services;
using Cash.Sample.Shared.Models;

namespace Cash.Ninject.Sample.Web.App_Start
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
