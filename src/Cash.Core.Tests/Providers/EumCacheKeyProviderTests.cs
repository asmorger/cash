using Cash.Core.Enums;
using Cash.Core.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Providers
{
    [TestClass]
    public class EumCacheKeyProviderTests : BaseCacheKeyProviderTests<EumCacheKeyProvider>
    {
        public override object[] GetSuccessArguments() => new object[] { CacheKeyProviderExecutionOrder.Enum };

        public override object[] GetFailureArguments() => new object[] { new Models.TestModelDefinition() };

        public override CacheKeyProviderExecutionOrder GetExecutionOrder() => CacheKeyProviderExecutionOrder.Enum;
    }
}
