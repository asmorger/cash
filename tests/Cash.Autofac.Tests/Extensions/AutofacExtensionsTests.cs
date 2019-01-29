// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;
using Autofac;

using Cash.Autofac.Extensions;
using Cash.Core.Interceptors;
using Cash.Core.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InvokeAsExtensionMethod

namespace Cash.Autofac.Tests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AutofacExtensionsTests
    {
        public ContainerBuilder Builder { get; set; }
        
        public ObjectCache Cache { get; set; }

        public ICacheKeyRegistry RegistrationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Builder = new ContainerBuilder();
            Cache = A.Fake<ObjectCache>();
            RegistrationService = A.Fake<ICacheKeyRegistry>();
        }

        [TestMethod]
        public void WithCaching_ExecutesSuccessfully()
        {
            try
            {
                Builder.RegisterType<CacheKeyRegistry>().As<ICacheKeyRegistry>().WithCaching();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception occured when none was expected. Message: '{ex.Message}'");
            }
        }
        
        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyGenerationService()
        {
            AutofacExtensions.AddCaching(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyRegistry>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyRegistrationService()
        {
            AutofacExtensions.AddCaching(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyRegistry>();

            Assert.IsNotNull(service);
            Assert.AreEqual(RegistrationService, service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheStore()
        {
            AutofacExtensions.AddCaching(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var cache = container.Resolve<ObjectCache>();

            Assert.IsNotNull(cache);
            Assert.AreEqual(Cache, cache);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheInterceptor()
        {
            AutofacExtensions.AddCaching(Builder, Cache, RegistrationService);

            var container = Builder.Build();
            var interceptor = container.Resolve<CachingInterceptor>();

            Assert.IsNotNull(interceptor);
        }
    }
}
