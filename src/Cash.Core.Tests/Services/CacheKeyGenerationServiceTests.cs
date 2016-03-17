// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

using Cash.Core.Exceptions;
using Cash.Core.Services;
using Cash.Core.Tests.Models;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Services
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CacheKeyGenerationServiceTests
    {
        private const string NullOrZeroArgumentsResult = "<no_arguments>";

        public ICacheKeyGenerationService CacheKeyGenerationService { get; set; }

        public ICacheKeyRegistrationService CacheKeyRegistrationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CacheKeyRegistrationService = A.Fake<ICacheKeyRegistrationService>();
            CacheKeyGenerationService = new CacheKeyGenerationService(CacheKeyRegistrationService);
        }

        [TestMethod]
        public void GetMethodCacheKey_CreatesProperKey_WhenDeclaringTypeIsKnown()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_NoParameters));

            var result = CacheKeyGenerationService.GetMethodCacheKey(methodInfo);
            var expectedResult = $"{model.GetType().FullName}.{methodInfo.Name}";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetMethodCacheKey_CreatesProperKey_WhenDeclaringTypeIsNotKnown()
        {
            var methodInfo = new MethodInfoWithNullDeclaringType();

            var result = CacheKeyGenerationService.GetMethodCacheKey(methodInfo);
            var expectedResult = $"<unknown>.{methodInfo.Name}";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForZeroArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { });

            Assert.AreEqual(NullOrZeroArgumentsResult, result);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForNullArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(null);

            Assert.AreEqual(NullOrZeroArgumentsResult, result);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForOneIntArgument()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5 });

            Assert.AreEqual("Int32::5", result);
        }

        /// <summary>
        /// Gets the arugment cache key_ creates proper key_ for two int arguments.
        /// </summary>
        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoIntArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, 10 });

            Assert.AreEqual("Int32::5||Int32::10", result);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoDisprateArguments()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, "test" });

            Assert.AreEqual("Int32::5||String::test", result);
        }

        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForUserDefinedType()
        {
            A.CallTo(() => CacheKeyRegistrationService.GetTypedCacheKeyProvider<TestModelDefinition>()).Returns(x => $"{x.Id}");
            A.CallTo(() => CacheKeyRegistrationService.IsProviderRegistered(typeof(TestModelDefinition))).Returns(true);

            var model = new TestModelDefinition { Id = 100 };
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { model });

            Assert.AreEqual("TestModelDefinition::100", result);
        }

        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForMultipleUserDefinedType()
        {
            A.CallTo(() => CacheKeyRegistrationService.GetTypedCacheKeyProvider<TestModelDefinition>()).Returns(x => $"{x.Id}");
            A.CallTo(() => CacheKeyRegistrationService.IsProviderRegistered(typeof(TestModelDefinition))).Returns(true);

            var model1 = new TestModelDefinition { Id = 100 };
            var model2 = new TestModelDefinition { Id = 500 };
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { model1, model2 });

            Assert.AreEqual("TestModelDefinition::100||TestModelDefinition::500", result);
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GetArgumentCacheKey_ThrowsAnException_WhenACacheKeyProviderHasNotBeenRegistered()
        {
            A.CallTo(() => CacheKeyRegistrationService.GetTypedCacheKeyProvider<TestModelDefinition>()).Throws(new UnregisteredCacheTypeException(typeof(TestModelDefinition)));
            A.CallTo(() => CacheKeyRegistrationService.IsProviderRegistered(typeof(TestModelDefinition))).Returns(true);

            var model = new TestModelDefinition { Id = 100 };
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { model });
        }

        [TestMethod]
        public void GetCacheKey_FormatsCacheKey_WhenNoParametersAreDefined()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_NoParameters));

            var result = CacheKeyGenerationService.GetCacheKey(methodInfo, null);
            var expectedResult = $"Cash.Core.Tests.Models.TestModelDefinition.TestMethod_NoParameters({NullOrZeroArgumentsResult})";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GetArugmentCacheKey_CreatesProperKey_ForTwoDisprateArgumentsOneIsNull()
        {
            var result = CacheKeyGenerationService.GetArgumentsCacheKey(new object[] { 5, null });

            Assert.AreEqual("Int32::5||[UnknownType]::[NULL]", result);
        }
    }
}