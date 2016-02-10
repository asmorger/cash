using Cash.Core.Services;
using Cash.Core.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

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

            result.ShouldBe(expectedResult);
        }

        [TestMethod]
        public void GetMethodCacheKey_CreatesProperKey_WhenDeclaringTypeIsNotKnown()
        {
            var methodInfo = new MethodInfoWithNullDeclaringType();

            var result = CacheKeyGenerationService.GetMethodCacheKey(methodInfo);
            var expectedResult = $"<unknown>.{methodInfo.Name}";

            result.ShouldBe(expectedResult);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForZeroArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] {});

            result.ShouldBe(NullOrZeroArgumentsResult);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForNullArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(null);

            result.ShouldBe(NullOrZeroArgumentsResult);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForOneIntArgument()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5 });

            result.ShouldBe("Int32::5");
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoIntArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, 10 });
            
            result.ShouldBe("Int32::5||Int32::10");
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoDisprateArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, "test" });

            result.ShouldBe("Int32::5||String::test");
        }
    }
}