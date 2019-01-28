// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

using Cash.Core.Exceptions;
using Cash.Core.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Services
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CacheKeyRegistrationServiceTests
    {
        public ICacheKeyRegistry Registry { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Registry = new CacheKeyRegistry();
        }

        [TestMethod]
        public void AddAndGetTypedCacheKeyProviders_PropertySetAndGet()
        {
            Registry.Register<int>(i => $"int_{i}");

            var targetProvider = Registry.Get<int>();
            Assert.IsNotNull(targetProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GetTypedCacheKeyProvider_ThrowsAnExceptionWhenAProviderHasNotBeenRegistered()
        {
            var targetProvider = Registry.Get<string>();
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateCacheFormatterRegistrationException))]
        public void AddTypedCacheKeyProvider_ThrowsAnExceptionWhenTheSameRegistrationTypeIsSet()
        {
            Registry.Register<int>(i => $"int_{i}");
            Registry.Register<int>(i => $"int2_{i}");
        }

        [TestMethod]
        public void IsProviderRegistered_ReturnsTrue_WhenProviderIsRegistered()
        {
            Registry.Register<int>(i => $"int_{i}");

            var result = Registry.HasRegistration(typeof(int));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsProviderRegistered_ReturnsFalse_WhenProviderIsNotRegistered()
        {
            Registry.Register<int>(i => $"int_{i}");

            var result = Registry.HasRegistration(typeof(string));

            Assert.IsFalse(result);
        }
    }
}