using Cash.Core.Attributes;

namespace Cash.Core.Tests.Models
{
    public class TestModelDefinition
    {
        public int Id { get; set; }

        public void TestMethod_NoParameters()
        {
        }

        public void TestMethod_OneSimpleParameter(int x)
        {
        }

        public void TestMethod_TwoSimpleParameters(double height, int age)
        {
        }

        [Cache]
        public void TestMethod_WithCacheAttribute()
        {
            
        }

        public void TestMethod_WithNoCacheAttribute()
        {
            
        }
    }
}