// // Copyright (c) Andrew Morger. All rights reserved.
// // Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System.Runtime.Caching;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests
{
    [TestClass]
    public class CacheContextTests
    {
        public ObjectCache FakeCache { get; set; }

        public CashContext CashContext { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            FakeCache = A.Fake<ObjectCache>();

            CashContext = CashContext.Instance;
        }

        [TestMethod]
        public void SetCacheProvider_SetsTheProperty()
        {
            CashContext.SetCacheBackingStore(FakeCache);
            Assert.IsNotNull(CashContext.CacheBackingStore);
        }
    }
}