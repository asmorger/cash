﻿// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Ninject;
using System.Runtime.Caching;

using Cash.Core.Services;

namespace Cash.Ninject
{
    public static class Cash
    {
        public static void RegisterCacheInfrastructure(IKernel kernel, ObjectCache cache, ICacheKeyRegistrationService cacheKeyRegistrationService)
        {
            kernel.Bind<ICacheKeyGenerationService>().To<CacheKeyGenerationService>();
            kernel.Bind<ICacheKeyRegistrationService>().ToConstant(cacheKeyRegistrationService);

            kernel.Bind<ObjectCache>().ToConstant(cache);
        }
    }
}
