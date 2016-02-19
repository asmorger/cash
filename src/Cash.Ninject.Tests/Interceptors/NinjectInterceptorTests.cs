using System.Runtime.Caching;

using Cash.Core.Services;
using Cash.Ninject.Interceptors;
using Cash.Ninject.Tests.Models;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Ninject.Tests.Interceptors
{
    [TestClass]
    public class NinjectInterceptorTests
    {
        public NinjectCachingInterceptor Interceptor { get; set; }

        public ObjectCache Cache { get; set; }

        public ICacheKeyGenerationService CacheKeyGenerationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Cache = A.Fake<ObjectCache>();
            CacheKeyGenerationService = A.Fake<ICacheKeyGenerationService>();
            Interceptor = new NinjectCachingInterceptor(Cache, CacheKeyGenerationService);
        }

        [TestMethod]
        public void GetCacheAttribute_ReturnsACacheAttributeWhenPresentOnAMethod()
        {
            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithCacheAttribute));
            var attribute = Interceptor.GetCacheAttribute(methodInfo);

            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        public void GetCacheAttribute_ReturnsNullWhenACacheAttributeIsNotPresentOnAMethod()
        {
            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithNoCacheAttribute));
            var attribute = Interceptor.GetCacheAttribute(methodInfo);

            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void GetCacheItem_ReturnsNotNullItem()
        {
            var item = Interceptor.GetCacheItem("key", "value");

            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void GetCacheItem_ResultingItemShouldSetKeyCorrectly()
        {
            var item = Interceptor.GetCacheItem("key", "value");

            Assert.AreEqual("key", item.Key);
        }

        [TestMethod]
        public void GetCacheItem_ResultingItemShouldSetValueCorrectly()
        {
            var item = Interceptor.GetCacheItem("key", "value");

            Assert.AreEqual("value", item.Value);
        }
    }
}
