﻿using System.Runtime.Caching;
using Autofac;
using Cash.Core.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Autofac.Tests
{
    [TestClass]
    public class CashTests
    {
        public ContainerBuilder Builder { get; set; }

        public ObjectCache Cache { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Builder = new ContainerBuilder();
            Cache = A.Fake<ObjectCache>();
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyGenerationService()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyGenerationService>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyRegistrationService()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyRegistrationService>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheStore()
        {
            Cash.RegisterCacheInfrastructure(Builder, Cache);

            var container = Builder.Build();
            var cache = container.Resolve<ObjectCache>();

            Assert.IsNotNull(cache);
            Assert.AreEqual(Cache, cache);
        }
    }
}