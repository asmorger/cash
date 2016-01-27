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
        public void GetTypedCacheKeyProvider_ReturnsNullWhenAProviderHasNotBeenRegistered()
        {
            var targetProvider = Service.GetTypedCacheKeyProvider<string>();
            Assert.IsNull(targetProvider);
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
