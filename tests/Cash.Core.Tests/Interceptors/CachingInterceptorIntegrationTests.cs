// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;

using Cash.Core.Interceptors;
using Cash.Core.Services;
using Cash.Core.Tests.Models;

using Castle.DynamicProxy;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Interceptors
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CachingInterceptorIntegrationTests
    {
        public CachingInterceptor Interceptor { get; set; }

        public ObjectCache Cache { get; set; }

        public ICacheKeyGenerationService CacheKeyGenerationService { get; set; }

        public IInvocation Invocation { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Cache = A.Fake<ObjectCache>();
            CacheKeyGenerationService = A.Fake<ICacheKeyGenerationService>();
            Invocation = A.Fake<IInvocation>();

            Interceptor = new CachingInterceptor(Cache, CacheKeyGenerationService);
        }

        [TestMethod]
        public void Intercept_CallsInvocationProceed_WhenThereIsNoCacheAttribute()
        {
            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithNoCacheAttribute));

            A.CallTo(() => Invocation.GetConcreteMethodInvocationTarget()).Returns(methodInfo);

            Interceptor.Intercept(Invocation);

            A.CallTo(() => Invocation.Proceed()).MustHaveHappened();
        }

        [TestMethod]
        public void Intercept_ChecksAndReturnsCachedContent_WhenTheCacheContainsTheKey()
        {
            const string cacheKey = "cacheKey(<no_arguments>)";
            const string region = null;
            var cacheOutput = new TestModelDefinition { Id = 100 };

            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithCacheAttribute));

            A.CallTo(() => Invocation.GetConcreteMethodInvocationTarget()).Returns(methodInfo);
            A.CallTo(() => CacheKeyGenerationService.GetCacheKey(methodInfo, A<object[]>.Ignored)).Returns(cacheKey);
            A.CallTo(() => Cache.Contains(cacheKey, region)).Returns(true);
            A.CallTo(() => Cache.Get(cacheKey, region)).Returns(cacheOutput);

            Interceptor.Intercept(Invocation);

            A.CallTo(() => Invocation.Proceed()).MustNotHaveHappened();
            A.CallTo(() => CacheKeyGenerationService.GetCacheKey(methodInfo, A<object[]>.Ignored)).MustHaveHappened();
            A.CallTo(() => Cache.Get(cacheKey, region)).MustHaveHappened();

            Assert.AreEqual(cacheOutput, Invocation.ReturnValue);
        }

        [TestMethod]
        public void Intercept_ChecksCacheAndInvokesThenCachesTheResult_WhenTheCacheDoesNotContainTheKey()
        {
            const string cacheKey = "cacheKey";
            const string region = null;
            var returnValue = new TestModelDefinition { Id = 500 };

            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithCacheAttribute));

            A.CallTo(() => Invocation.GetConcreteMethodInvocationTarget()).Returns(methodInfo);
            A.CallTo(() => CacheKeyGenerationService.GetCacheKey(methodInfo, A<object[]>.Ignored)).Returns(cacheKey);
            A.CallTo(() => Cache.Contains(cacheKey, region)).Returns(false);
            A.CallTo(() => Invocation.ReturnValue).Returns(returnValue);

            Interceptor.Intercept(Invocation);

            A.CallTo(() => Invocation.Proceed()).MustHaveHappened();
            A.CallTo(() => CacheKeyGenerationService.GetCacheKey(methodInfo, A<object[]>.Ignored)).MustHaveHappened();
            A.CallTo(() => Cache.Set(A<CacheItem>.Ignored, A<CacheItemPolicy>.Ignored)).MustHaveHappened();
        }
    }
}