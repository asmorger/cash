using System.Runtime.Caching;
using Cash.Core.Exceptions;
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
