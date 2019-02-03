// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

using Cash.Core.Interceptors;
using Cash.Core.Tests.Models;

using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Interceptors
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CachingInterceptorTests
    {
        public CachingInterceptor Interceptor { get; set; }

        public IMemoryCache Cache { get; set; }

        public ICacheKeyGenerator Generator { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Cache = A.Fake<IMemoryCache>();
            Generator = A.Fake<ICacheKeyGenerator>();
            Interceptor = new CachingInterceptor(Cache, Generator);
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
    }
}