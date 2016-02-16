// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System;

using Cash.Core.Exceptions;
using Cash.Core.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Services
{
    [TestClass]
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
        public void GetTypedCacheKeyProvider_ThrowsAnExceptionWhenAProviderHasNotBeenRegistered()
        {
            try
            {
                var targetProvider = Service.GetTypedCacheKeyProvider<string>();
            }
            catch (Exception e)
            {
                if (e is UnregisteredCacheTypeException)
                {
                    Assert.IsNotNull(e);
                    return;
                }

                Assert.Fail($"Expected exception type was 'UnregisteredCacheTypeException'.  What was thrown was '{e.GetType()}'");
            }
        }

        [TestMethod]
        public void AddTypedCacheKeyProvider_ThrowsAnExceptionWhenTheSameRegistrationTypeIsSet()
        {
            try
            {
                Service.AddTypedCacheKeyProvider<int>(i => $"int_{i}");
                Service.AddTypedCacheKeyProvider<int>(i => $"int2_{i}");

                Assert.Fail("Error should have been thrown here.");
            }
            catch (Exception e)
            {
                if (e is DuplicateCacheProviderRegistrationException)
                {
                    Assert.IsNotNull(e);
                    return;
                }

                Assert.Fail($"Expected exception type was 'DuplicateCacheProviderRegistrationException'.  What was thrown was '{e.GetType()}'");
            }
        }
    }
}