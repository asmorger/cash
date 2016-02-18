using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

using Cash.Core.Services;

namespace Cash.Ninject
{
    public static class Cash
    {
        public static void RegisterCacheInfrastructure(StandardKernel kernel, ObjectCache cache)
        {
            kernel.Bind<ICacheKeyGenerationService>().To<CacheKeyGenerationService>();
            kernel.Bind<ICacheKeyRegistrationService>().To<CacheKeyRegistrationService>();

            kernel.Bind<ObjectCache>().ToConstant(cache);
        }
    }
}
