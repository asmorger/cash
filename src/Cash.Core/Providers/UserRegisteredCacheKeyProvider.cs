using Cash.Core.Providers.Base;
using Cash.Core.Services;

namespace Cash.Core.Providers
{
    public class UserRegisteredCacheKeyProvider : BaseRegisteredCacheKeyProvider
    {
        public UserRegisteredCacheKeyProvider(ICacheKeyRegistry cacheKeyRegistry)
            : base(cacheKeyRegistry)
        {
        }

        public override bool IsValid(object parameter)
        {
            var type = parameter.GetType();
            var output = CacheKeyRegistry.HasRegistration(type);
            return output;
        }
    }
}
