// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;

using Cash.Autofac.Extensions;
using Cash.Core;
using Cash.Core.Interceptors;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InvokeAsExtensionMethod

namespace Cash.Autofac.Tests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AutofacExtensionsTests
    {
        public ContainerBuilder Builder { get; set; }
        
        public IMemoryCache Cache { get; set; }

        public ICacheKeyRegistry RegistrationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Builder = new ContainerBuilder();
            Cache = A.Fake<IMemoryCache>();
            RegistrationService = A.Fake<ICacheKeyRegistry>();

            // simulate the .net core registration
            Builder.Register(c => new MemoryCache(new MemoryCacheOptions())).As<IMemoryCache>();
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
            AutofacExtensions.AddCaching(Builder, RegistrationService);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyRegistry>();

            Assert.IsNotNull(service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheKeyRegistrationService()
        {
            AutofacExtensions.AddCaching(Builder, RegistrationService);

            var container = Builder.Build();
            var service = container.Resolve<ICacheKeyRegistry>();

            Assert.IsNotNull(service);
            Assert.AreEqual(RegistrationService, service);
        }

        [TestMethod]
        public void RegisterCacheInfrastructure_RegistersTheCacheInterceptor()
        {
            AutofacExtensions.AddCaching(Builder, RegistrationService);

            var container = Builder.Build();
            var interceptor = container.Resolve<CachingInterceptor>();

            Assert.IsNotNull(interceptor);
        }
    }
}
