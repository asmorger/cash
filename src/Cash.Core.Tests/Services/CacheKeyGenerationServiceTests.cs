using Cash.Core.Services;
using Cash.Core.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Services
{
    [TestClass]
    public class CacheKeyGenerationServiceTests
    {
        private const string NullOrZeroArgumentsResult = "<no_arguments>";

        public ICacheKeyGenerationService CacheKeyGenerationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CacheKeyGenerationService = new CacheKeyGenerationService();
        }

        [TestMethod]
        public void GetMethodCacheKey_CreatesProperKey_WhenDeclaringTypeIsKnown()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_NoParameters));

            var result = CacheKeyGenerationService.GetMethodCacheKey(methodInfo);
            var expectedResult = $"{model.GetType().FullName}.{methodInfo.Name}";

            Assert.AreEqual(result, expectedResult);
            // result.ShouldBe(expectedResult);
        }

        [TestMethod]
        public void GetMethodCacheKey_CreatesProperKey_WhenDeclaringTypeIsNotKnown()
        {
            var methodInfo = new MethodInfoWithNullDeclaringType();

            var result = CacheKeyGenerationService.GetMethodCacheKey(methodInfo);
            var expectedResult = $"<unknown>.{methodInfo.Name}";

            Assert.AreEqual(result, expectedResult);
            // result.ShouldBe(expectedResult);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForZeroArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] {});

            Assert.AreEqual(result, NullOrZeroArgumentsResult);
            // result.ShouldBe(NullOrZeroArgumentsResult);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForNullArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(null);

            Assert.AreEqual(result, NullOrZeroArgumentsResult);
            // result.ShouldBe(NullOrZeroArgumentsResult);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForOneIntArgument()
        {
            CashContext.Instance.RegistrationService.ClearCacheKeyProviders();
            CashContext.Instance.RegistrationService.AddTypedCacheKeyProvider<int>(x => $"{x}");
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5 });

            Assert.AreEqual("Int32::5", result);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoIntArguments()
        {
            CashContext.Instance.RegistrationService.ClearCacheKeyProviders();
            CashContext.Instance.RegistrationService.AddTypedCacheKeyProvider<int>(x => $"{x}");
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, 10 });

            Assert.AreEqual("Int32::5||Int32::10", result);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoDisprateArguments()
        {
            // todo: remove singleton in favor of complete constructor injection
            CashContext.Instance.RegistrationService.ClearCacheKeyProviders();
            CashContext.Instance.RegistrationService.AddTypedCacheKeyProvider<int>(x => $"{x}");
            CashContext.Instance.RegistrationService.AddTypedCacheKeyProvider<string>(x => $"{x}");

            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, "test" });

            Assert.AreEqual("Int32::5||String::test", result);
        }
    }
}