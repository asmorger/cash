using System.Linq;

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

        [TestMethod]
        public void GetValueRepresentation_ReturnsExpectedValues()
        {
            var argument = CacheKeyProviderExecutionOrder.Enum;
            var result = CacheKeyProvider.GetValueRepresentation(argument);
            var expectedResult = ((int)argument).ToString();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetTypeNameRepresentation_ReturnsExpectedValues()
        {
            var argument = CacheKeyProviderExecutionOrder.Enum;
            var result = CacheKeyProvider.GetTypeNameRepresentation(argument);
            var expectedResult = $"Enum[{argument.GetType().Name}]";
            Assert.AreEqual(expectedResult, result);
        }
    }
}
