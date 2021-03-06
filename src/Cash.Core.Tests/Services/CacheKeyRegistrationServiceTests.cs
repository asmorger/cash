﻿// Copyright (c) Andrew Morger. All rights reserved.
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
            Service.RegisterCacheKeyFormatter<int>(i => $"int_{i}");

            var targetProvider = Service.GetCacheKeyFormatter<int>();
            Assert.IsNotNull(targetProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(UnregisteredCacheTypeException))]
        public void GetTypedCacheKeyProvider_ThrowsAnExceptionWhenAProviderHasNotBeenRegistered()
        {
            var targetProvider = Service.GetCacheKeyFormatter<string>();
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateCacheFormatterRegistrationException))]
        public void AddTypedCacheKeyProvider_ThrowsAnExceptionWhenTheSameRegistrationTypeIsSet()
        {
            Service.RegisterCacheKeyFormatter<int>(i => $"int_{i}");
            Service.RegisterCacheKeyFormatter<int>(i => $"int2_{i}");
        }

        [TestMethod]
        public void IsProviderRegistered_ReturnsTrue_WhenProviderIsRegistered()
        {
            Service.RegisterCacheKeyFormatter<int>(i => $"int_{i}");

            var result = Service.IsFormatterRegistered(typeof(int));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsProviderRegistered_ReturnsFals_WhenProviderIsNotRegistered()
        {
            Service.RegisterCacheKeyFormatter<int>(i => $"int_{i}");

            var result = Service.IsFormatterRegistered(typeof(string));

            Assert.IsFalse(result);
        }
    }
}