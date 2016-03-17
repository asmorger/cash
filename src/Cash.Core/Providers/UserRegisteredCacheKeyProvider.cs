using Cash.Core.Enums;
using Cash.Core.Providers.Base;
using Cash.Core.Services;

namespace Cash.Core.Providers
{
    public class UserRegisteredCacheKeyProvider : BaseRegisteredCacheKeyProvider
    {
        public UserRegisteredCacheKeyProvider(ICacheKeyRegistrationService cacheKeyRegistrationService)
            : base(cacheKeyRegistrationService)
        {
        }

        public override CacheKeyProviderExecutionOrder ExecutionOrder => CacheKeyProviderExecutionOrder.UserRegistered;

        public override bool IsValid(object parameter)
        {
            var type = parameter.GetType();
            var output = CacheKeyRegistrationService.IsProviderRegistered(type);
            return output;
        }
    }
}
