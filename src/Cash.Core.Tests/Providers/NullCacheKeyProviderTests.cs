using Cash.Core.Enums;
using Cash.Core.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Providers
{
    [TestClass]
    public class NullCacheKeyProviderTests : BaseCacheKeyProviderTests<NullCacheKeyProvider>
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        public override object[] GetSuccessArguments() => new object[] { null };

        public override object[] GetFailureArguments() => new object[] { 1, "test", new Models.TestModelDefinition() };

        public override CacheKeyProviderExecutionOrder GetExecutionOrder() => CacheKeyProviderExecutionOrder.Null;
    }
}
