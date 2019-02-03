// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

using Cash.Core.Interceptors;
using Cash.Core.Tests.Models;

using Castle.DynamicProxy;

using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Interceptors
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CachingInterceptorIntegrationTests
    {
        public CachingInterceptor Interceptor { get; set; }

        public IMemoryCache Cache { get; set; }

        public ICacheKeyGenerator Generator { get; set; }

        public IInvocation Invocation { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Cache = A.Fake<IMemoryCache>();
            Generator = A.Fake<ICacheKeyGenerator>();
            Invocation = A.Fake<IInvocation>();

            Interceptor = new CachingInterceptor(Cache, Generator);
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
            var cacheOutput = new TestModelDefinition { Id = 100 };

            object ignored = null;

            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithCacheAttribute));

            A.CallTo(() => Invocation.GetConcreteMethodInvocationTarget()).Returns(methodInfo);
            A.CallTo(() => Generator.Generate(methodInfo, A<object[]>.Ignored)).Returns(cacheKey);
            
            // force the call to use object so it selects the interface implementation, not the generic extension method
            A.CallTo(() => Cache.TryGetValue((object)cacheKey, out ignored)).Returns(true).AssignsOutAndRefParameters(cacheOutput);

            Interceptor.Intercept(Invocation);

            A.CallTo(() => Invocation.Proceed()).MustNotHaveHappened();
            A.CallTo(() => Generator.Generate(methodInfo, A<object[]>.Ignored)).MustHaveHappened();
            A.CallTo(() => Cache.CreateEntry(A<object>.Ignored)).MustNotHaveHappened();

            Assert.AreEqual(cacheOutput, Invocation.ReturnValue);
        }

        [TestMethod]
        public void Intercept_ChecksCacheAndInvokesThenCachesTheResult_WhenTheCacheDoesNotContainTheKey()
        {
            const string cacheKey = "cacheKey";
            object ignored = null;
            var returnValue = new TestModelDefinition { Id = 500 };

            var methodInfo =
                typeof(TestModelDefinition).GetMethod(nameof(TestModelDefinition.TestMethod_WithCacheAttribute));

            A.CallTo(() => Invocation.GetConcreteMethodInvocationTarget()).Returns(methodInfo);
            A.CallTo(() => Generator.Generate(methodInfo, A<object[]>.Ignored)).Returns(cacheKey);
            
            // force the call to use object so it selects the interface implementation, not the generic extension method
            A.CallTo(() => Cache.TryGetValue((object)cacheKey, out ignored)).Returns(false);
            A.CallTo(() => Invocation.ReturnValue).Returns(returnValue);

            Interceptor.Intercept(Invocation);

            A.CallTo(() => Invocation.Proceed()).MustHaveHappened();
            A.CallTo(() => Generator.Generate(methodInfo, A<object[]>.Ignored)).MustHaveHappened();
            A.CallTo(() => Cache.CreateEntry(A<object>.Ignored)).MustHaveHappened();
        }
    }
}