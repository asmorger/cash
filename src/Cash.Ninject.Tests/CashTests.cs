using System.Runtime.Caching;

using Cash.Core.Services;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ninject;

namespace Cash.Ninject.Tests
{
    [TestClass]
    public class CashTests
    {
        public StandardKernel Kernel { get; set; }

        public ObjectCache Cache { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Kernel = new StandardKernel();
            Cache = A.Fake<ObjectCache>();
        }


        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyGenerationService()
        {
            Cash.RegisterCacheInfrastructure(Kernel, Cache);

            var canResolve = Kernel.CanResolve<ICacheKeyGenerationService>();

            Assert.IsTrue(canResolve);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyRegistrationService()
        {
            Cash.RegisterCacheInfrastructure(Kernel, Cache);

            var canResolve = Kernel.CanResolve<ICacheKeyRegistrationService>();

            Assert.IsTrue(canResolve);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheStore()
        {
            Cash.RegisterCacheInfrastructure(Kernel, Cache);

            var canResolve = Kernel.CanResolve<ObjectCache>();

            Assert.IsTrue(canResolve);

            // Assert.IsNotNull(cache);
            // Assert.AreEqual(Cache, cache);
        }
    }
}
