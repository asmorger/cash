// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System.Runtime.Caching;

using Cash.Core.Interceptors;
using Cash.Core.Services;
using Cash.Core.Tests.Models;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shouldly;

namespace Cash.Core.Tests.Interceptors
{
    [TestClass]
    public class CachingInterceptorTests
    {
        public CachingInterceptor Interceptor { get; set; }

        public ObjectCache Cache { get; set; }

        public ICacheKeyGenerationService CacheKeyGenerationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Cache = A.Fake<ObjectCache>();
            CacheKeyGenerationService = A.Fake<ICacheKeyGenerationService>();
            Interceptor = new CachingInterceptor(Cache, CacheKeyGenerationService);
        }

        [TestMethod]
        public void GetCacheAttribute_ReturnsACacheAttributeWhenPresentOnAMethod()
        {
            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithCacheAttribute));
            var attribute = Interceptor.GetCacheAttribute(methodInfo);

            attribute.ShouldNotBeNull();
        }

        [TestMethod]
        public void GetCacheAttribute_ReturnsNullWhenACacheAttributeIsNotPresentOnAMethod()
        {
            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithNoCacheAttribute));
            var attribute = Interceptor.GetCacheAttribute(methodInfo);

            attribute.ShouldBeNull();
        }

        [TestMethod]
        public void GetCacheItem_ReturnsNotNullItem()
        {
            var item = Interceptor.GetCacheItem("key", "value");

            item.ShouldNotBeNull();
        }

        [TestMethod]
        public void GetCacheItem_ResultingItemShouldSetKeyCorrectly()
        {
            var item = Interceptor.GetCacheItem("key", "value");

            item.Key.ShouldBe("key");
        }

        [TestMethod]
        public void GetCacheItem_ResultingItemShouldSetValueCorrectly()
        {
            var item = Interceptor.GetCacheItem("key", "value");

            item.Value.ShouldBe("value");
        }
    }
}