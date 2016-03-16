using Cash.Core.Enums;
using Cash.Core.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Providers
{
    [TestClass]
    public class PrimitivieTypeCacheKeyProviderTests : BaseCacheKeyProviderTests<PrimitiveTypeCacheKeyProvider>
    {
        public override object[] GetSuccessArguments() => new object[] { 1, 1.0, "test", (float)1 };

        public override object[] GetFailureArguments() => new object[] { new Models.TestModelDefinition() };

        public override CacheKeyProviderExecutionOrder GetExecutionOrder() => CacheKeyProviderExecutionOrder.Primitive;
    }
}
