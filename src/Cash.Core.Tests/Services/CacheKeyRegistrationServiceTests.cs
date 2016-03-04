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
        public CacheKeyRegistrationService Service { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Service = new CacheKeyRegistrationService();
        }

        [TestMethod]
        public void AddAndGetTypedCacheKeyProviders_PropertySetAndGet()
        {
            Service.AddTypedCacheKeyProvider<int>(i => $"int_{i}");

            var targetProvider = Service.GetTypedCacheKeyProvider<int>();
            Assert.IsNotNull(targetProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GetTypedCacheKeyProvider_ThrowsAnExceptionWhenAProviderHasNotBeenRegistered()
        {
            var targetProvider = Service.GetTypedCacheKeyProvider<string>();
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateCacheProviderRegistrationException))]
        public void AddTypedCacheKeyProvider_ThrowsAnExceptionWhenTheSameRegistrationTypeIsSet()
        {
            Service.AddTypedCacheKeyProvider<int>(i => $"int_{i}");
            Service.AddTypedCacheKeyProvider<int>(i => $"int2_{i}");
        }
    }
}