using Cash.Core.Exceptions;
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

        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForUserDefinedType()
        {
            CashContext.Instance.RegistrationService.ClearCacheKeyProviders();
            CashContext.Instance.RegistrationService.AddTypedCacheKeyProvider<TestModelDefinition>(x => $"{x.Id}");

            var model = new TestModelDefinition { Id = 100 };
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { model });

            result.ShouldBe("TestModelDefinition::100");
        }

        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForMultipleUserDefinedType()
        {
            CashContext.Instance.RegistrationService.ClearCacheKeyProviders();
            CashContext.Instance.RegistrationService.AddTypedCacheKeyProvider<TestModelDefinition>(x => $"{x.Id}");

            var model1 = new TestModelDefinition { Id = 100 };
            var model2 = new TestModelDefinition { Id = 500 };
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { model1, model2 });

            result.ShouldBe("TestModelDefinition::100||TestModelDefinition::500");
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GetArgumentCacheKey_ThrowsAnException_WhenACacheKeyProviderHasNotBeenRegistered()
        {
            CashContext.Instance.RegistrationService.ClearCacheKeyProviders();

            var model = new TestModelDefinition { Id = 100 };
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { model });
        }
    }
}