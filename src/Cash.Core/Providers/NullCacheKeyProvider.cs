using Cash.Core.Enums;
using Cash.Core.Providers.Base;

namespace Cash.Core.Providers
{
    public sealed class NullCacheKeyProvider : BaseCacheKeyProvider
    {
        public override CacheKeyProviderExecutionOrder ExecutionOrder => CacheKeyProviderExecutionOrder.Null;

        public override bool IsValid(object parameter)
        {
            var output = parameter == null;
            return output;
        }

        public override string GetValueRepresentation(object parameter) => "[NULL]";

        public override string GetTypeNameRepresentation(object parameter) => "[UnknownType]";
    }
}
