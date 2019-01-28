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

        public ICacheKeyGenerator Generator { get; set; }

        public ICacheKeyRegistry Registry { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Registry = A.Fake<ICacheKeyRegistry>();
            Generator = new CacheKeyGenerator(Registry);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForZeroArguments()
        {
            var result = Generator.GenerateForArguments(new object[] { });

            Assert.AreEqual(NullOrZeroArgumentsResult, result);
        }

        [TestMethod]
        public void GetArgumentsCacheKey_CreatesProperKey_ForNullArguments()
        {
            var result = Generator.GenerateForArguments(null);

            Assert.AreEqual(NullOrZeroArgumentsResult, result);
        }

        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForOneIntArgument()
        {
            var result = Generator.GenerateForArguments(new object[] { 5 });

            Assert.AreEqual("Int32::5", result);
        }

        /// <summary>
        /// Gets the Argument cache key_ creates proper key_ for two int arguments.
        /// </summary>
        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForTwoIntArguments()
        {
            var result = Generator.GenerateForArguments(new object[] { 5, 10 });

            Assert.AreEqual("Int32::5||Int32::10", result);
        }

        [TestMethod]
        public void GetArgumentCacheKey_CreatesProperKey_ForTwoDisprateArguments()
        {
            var result = Generator.GenerateForArguments(new object[] { 5, "test" });

            Assert.AreEqual("Int32::5||String::test", result);
        }

        [TestMethod]
        public void GenerateForArguments_CreatesProperKey_ForUserDefinedType()
        {
            A.CallTo(() => Registry.Get<TestModelDefinition>()).Returns(x => $"{x.Id}");
            A.CallTo(() => Registry.HasRegistration(typeof(TestModelDefinition))).Returns(true);

            var model = new TestModelDefinition { Id = 100 };
            var result = Generator.GenerateForArguments(new object[] { model });

            Assert.AreEqual("TestModelDefinition::100", result);
        }

        [TestMethod]
        public void GenerateForArguments_CreatesProperKey_ForMultipleUserDefinedType()
        {
            A.CallTo(() => Registry.Get<TestModelDefinition>()).Returns(x => $"{x.Id}");
            A.CallTo(() => Registry.HasRegistration(typeof(TestModelDefinition))).Returns(true);

            var model1 = new TestModelDefinition { Id = 100 };
            var model2 = new TestModelDefinition { Id = 500 };
            var result = Generator.GenerateForArguments(new object[] { model1, model2 });

            Assert.AreEqual("TestModelDefinition::100||TestModelDefinition::500", result);
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GenerateForArguments_ThrowsAnException_WhenACacheKeyProviderHasNotBeenRegistered()
        {
            A.CallTo(() => Registry.Get<TestModelDefinition>()).Throws(new UnregisteredCacheTypeException(typeof(TestModelDefinition)));
            A.CallTo(() => Registry.HasRegistration(typeof(TestModelDefinition))).Returns(true);

            var model = new TestModelDefinition { Id = 100 };
            var result = Generator.GenerateForArguments(new object[] { model });
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GetArgumentsCacheKey_ThrowsAnException_WhenNoProviderIsRegistered()
        {
            A.CallTo(() => Registry.HasRegistration(typeof(TestModelDefinition))).Returns(false);

            var model = new TestModelDefinition { Id = 100 };
            var result = Generator.GenerateForArguments(new object[] { model });
        }

        [TestMethod]
        public void Generate_FormatsCacheKey_WhenNoParametersAreDefined()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_NoParameters));

            var result = Generator.Generate(methodInfo, null);
            var expectedResult = $"Cash.Core.Tests.Models.TestModelDefinition.TestMethod_NoParameters({NullOrZeroArgumentsResult})";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Generate_FormatsCacheKey_WhenOneParametersIsDefined()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_OneSimpleParameter));

            var result = Generator.Generate(methodInfo, new object[] { 10 });
            var expectedResult = $"Cash.Core.Tests.Models.TestModelDefinition.TestMethod_OneSimpleParameter(Int32::10)";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Generate_FormatsCacheKey_WhenEnumerableParameterIsDefined()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_EnumerableParameter));

            var result = Generator.Generate(methodInfo, new object[] { 10, 20 });
            var expectedResult = $"Cash.Core.Tests.Models.TestModelDefinition.TestMethod_EnumerableParameter(Int32::10||Int32::20)";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Generate_FormatsCacheKey_WhenEnumerableOutputIsDefined_AndNoParameters()
        {
            var model = new TestModelDefinition();
            var methodInfo = model.GetType().GetMethod(nameof(model.TestMethod_EnumerableReturn));

            var result = Generator.Generate(methodInfo, null);
            var expectedResult = $"Cash.Core.Tests.Models.TestModelDefinition.TestMethod_EnumerableReturn<Int32>({NullOrZeroArgumentsResult})";

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void GenerateForArguments_CreatesProperKey_ForTwoDisprateArgumentsOneIsNull()
        {
            var result = Generator.GenerateForArguments(new object[] { 5, null });

            Assert.AreEqual("Int32::5||[UnknownType]::[NULL]", result);
        }
    }
}