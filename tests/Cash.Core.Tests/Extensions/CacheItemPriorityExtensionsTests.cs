// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Cash.Core.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Extensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CacheItemPriorityExtensionsTests
    {
        [TestMethod]
        public void Convert_ProperlyConverts_NotRemovable()
        {
            var result = CacheItemPriorityExtensions.Convert(CacheItemPriority.NeverRemove);

            Assert.AreEqual(System.Runtime.Caching.CacheItemPriority.NotRemovable, result);
        }

        [TestMethod]
        public void Convert_ProperlyConverts_High()
        {
            var result = CacheItemPriorityExtensions.Convert(CacheItemPriority.High);

            Assert.AreEqual(System.Runtime.Caching.CacheItemPriority.Default, result);
        }

        [TestMethod]
        public void Convert_ProperlyConverts_Normal()
        {
            var result = CacheItemPriorityExtensions.Convert(CacheItemPriority.Normal);

            Assert.AreEqual(System.Runtime.Caching.CacheItemPriority.Default, result);
        }

        [TestMethod]
        public void Convert_ProperlyConverts_Low()
        {
            var result = CacheItemPriorityExtensions.Convert(CacheItemPriority.Low);

            Assert.AreEqual(System.Runtime.Caching.CacheItemPriority.Default, result);
        }
    }
}
