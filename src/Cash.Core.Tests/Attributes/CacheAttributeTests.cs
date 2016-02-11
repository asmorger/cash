using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cash.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Cash.Core.Tests.Attributes
{
    [TestClass]
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

            attribute.Priority.ShouldBe(CacheItemPriority.Normal);
        }

        [TestMethod]
        public void Ctor_SetsThePriority_ToWhatIsPassedIn()
        {
            var attribute = new CacheAttribute(CacheItemPriority.High);

            attribute.Priority.ShouldBe(CacheItemPriority.High);
        }
    }
}
