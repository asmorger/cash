// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using Cash.Autofac.Extensions;
using Cash.Core.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Autofac.Tests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AutofacExtensionsTests
    {
        public ContainerBuilder Builder { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Builder = new ContainerBuilder();
        }

        [TestMethod]
        public void WithDefaultCache_ExecutesSuccessfully()
        {
            try
            {
                Builder.RegisterType<CacheKeyGenerationService>().As<ICacheKeyGenerationService>().WithDefaultCache();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception occured when none was expected. Message: '{ex.Message}'");
            }
        }
    }
}
