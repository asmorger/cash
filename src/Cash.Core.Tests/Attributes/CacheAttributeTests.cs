// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

using Cash.Core.Attributes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Attributes
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CacheAttributeTests
    {
        public CacheAttribute CacheAttribute { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CacheAttribute = new CacheAttribute();
        }

        [TestMethod]
        public void Ctor_DefaultsThePriority_ToNormal()
        {
            var attribute = new CacheAttribute();

            Assert.AreEqual(CacheItemPriority.Normal, attribute.Priority);
        }

        [TestMethod]
        public void Ctor_SetsThePriority_ToWhatIsPassedIn()
        {
            var attribute = new CacheAttribute(CacheItemPriority.High);

            Assert.AreEqual(CacheItemPriority.High, attribute.Priority);
        }
    }
}