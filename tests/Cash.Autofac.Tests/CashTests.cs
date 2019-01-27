// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;
using Autofac;

using Cash.Core.Interceptors;
using Cash.Core.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Autofac.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CashTests
    {
        public ContainerBuilder Builder { get; set; }

        public ObjectCache Cache { get; set; }

        public ICacheKeyRegistrationService RegistrationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Builder = new ContainerBuilder();
            Cache = A.Fake<ObjectCache>();
            RegistrationService = A.Fake<ICacheKeyRegistrationService>();
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyGenerationService()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyGenerationService>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyRegistrationService()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyRegistrationService>();

            Assert.IsNotNull(service);
            Assert.AreEqual(RegistrationService, service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheStore()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var cache = container.Resolve<ObjectCache>();

            Assert.IsNotNull(cache);
            Assert.AreEqual(Cache, cache);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheInterceptor()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var interceptor = container.Resolve<CachingInterceptor>();

            Assert.IsNotNull(interceptor);
        }
    }
}
