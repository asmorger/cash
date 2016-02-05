using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cash.Core.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Services
{
    [TestClass]
    public class CacheKeyGenerationServiceTests
    {
        public ICacheKeyGenerationService CacheKeyGenerationService { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CacheKeyGenerationService = new CacheKeyGenerationService();
        }
    }
}
