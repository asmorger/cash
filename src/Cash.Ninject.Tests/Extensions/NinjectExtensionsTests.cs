// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Cash.Core.Services;
using Cash.Ninject.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ninject;

namespace Cash.Ninject.Tests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class NinjectExtensionsTests
    {
        public StandardKernel Kernel { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Kernel = new StandardKernel();
        }

        [TestMethod]
        public void WithDefaultCache_ExecutesSuccessfully()
        {
            try
            {
                Kernel.Bind<ICacheKeyGenerationService>().To<CacheKeyGenerationService>().InThreadScope().WithDefaultCache();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Exception occured when none was expected. Message: '{ex.Message}'");
            }
        }
    }
}
