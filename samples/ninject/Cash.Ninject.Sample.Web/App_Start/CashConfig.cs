using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cash.Core.Services;

namespace Cash.Ninject.Sample.Web.App_Start
{
    public static class CashConfig
    {
        public static ICacheKeyRegistrationService RegisterCacheKeys()
        {
            var registrationService = new CacheKeyRegistrationService();

            // registrationService.AddTypedCacheKeyProvider<UserModel>(x => $"{x.Id}");

            return registrationService;
        }
    }
}
