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

        public CashContext CashContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            FakeCache = A.Fake<ObjectCache>();

            CashContext = CashContext.Instance;

            CashContext.ClearCacheProviders();
            CashContext.SetCacheProvider(FakeCache);
        }

        [TestMethod]
        public void SetCacheProvider_SetsTheProperty()
        {
            Assert.IsNotNull(CashContext.CacheProvider);
        }

        [TestMethod]
        public void AddAndGetTypedCacheKeyProviders_PropertySetAndGet()
        {
            CashContext.AddTypedCacheKeyProvider<int>(i => $"int_{i}");

            var targetProvider = CashContext.GetTypedCacheKeyProvider<int>();
            Assert.IsNotNull(targetProvider);
        }

        [TestMethod]
        public void GetTypedCacheKeyProvider_ReturnsNullWhenAProviderHasNotBeenRegistered()
        {
            var targetProvider = CashContext.GetTypedCacheKeyProvider<string>();
            Assert.IsNull(targetProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateCacheProviderRegistrationException))]
        public void AddTypedCacheKeyProvider_ThrowsAnExceptionWhenTheSameRegistrationTypeIsSet()
        {
            CashContext.AddTypedCacheKeyProvider<int>(i => $"int_{i}");
            CashContext.AddTypedCacheKeyProvider<int>(i => $"int2_{i}");
        }
    }
}
