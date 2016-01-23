using System;
using System.Runtime.Caching;
using Cash.Core.Exceptions;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests
{
    [TestClass]
    public class CacheContextTests
    {
        public ObjectCache FakeCache { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            FakeCache = A.Fake<ObjectCache>();

            CashContext.ClearCacheProviders();
            CashContext.SetCacheProvider(FakeCache);
        }

        [TestMethod]
        public void SetCacheProvider_SetsTheProperty()
        {
            Assert.IsNotNull(CashContext.CacheProvider);
        }

        [TestMethod]
        public void AddAndGetProviders_PropertySetAndGet()
        {
            CashContext.AddProvider<int>(i => $"int_{i}");

            var targetProvider = CashContext.GetProvider<int>();
            Assert.IsNotNull(targetProvider);
        }

        [TestMethod]
        public void GetProvider_ReturnsNullWhenAProviderHasNotBeenRegistered()
        {
            var targetProvider = CashContext.GetProvider<string>();
            Assert.IsNull(targetProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateCacheProviderRegistrationException))]
        public void AddProvider_ThrowsAnExceptionWhenTheSameRegistrationTypeIsSet()
        {
            CashContext.AddProvider<int>(i => $"int_{i}");
            CashContext.AddProvider<int>(i => $"int2_{i}");
        }
    }
}
