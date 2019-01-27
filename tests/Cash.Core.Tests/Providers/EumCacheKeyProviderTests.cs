using Cash.Core.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Providers
{
    public enum TestEnum
    {
        Success
    }
    
    [TestClass]
    public class EumCacheKeyProviderTests : BaseCacheKeyProviderTests<EumCacheKeyProvider>
    {
        public override object[] GetSuccessArguments() => new object[] { TestEnum.Success };

        public override object[] GetFailureArguments() => new object[] { new Models.TestModelDefinition() };

        [TestMethod]
        public void GetValueRepresentation_ReturnsExpectedValues()
        {
            var argument = TestEnum.Success;
            var result = CacheKeyProvider.GetValueRepresentation(argument);
            var expectedResult = ((int)argument).ToString();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetTypeNameRepresentation_ReturnsExpectedValues()
        {
            var argument = TestEnum.Success;
            var result = CacheKeyProvider.GetTypeNameRepresentation(argument);
            var expectedResult = $"Enum[{argument.GetType().Name}]";
            Assert.AreEqual(expectedResult, result);
        }
    }
}
