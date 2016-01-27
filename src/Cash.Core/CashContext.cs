﻿using System;
using System.Linq.Expressions;
using System.Runtime.Caching;
using Cash.Core.Services;

namespace Cash.Core
{
    public class CashContext
    {
        private static volatile CashContext instance;
        private static readonly object CreationLock = new object();

        public readonly ICacheKeyRegistrationService RegistrationService;

        public static CashContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (CreationLock)
                    {
                        if (instance == null)
                        {
                            instance = new CashContext();
                        }
                    }
                }

                return instance;
            }
        }

        private CashContext()
        {
            RegistrationService = new CacheKeyRegistrationService();
        }

        public ObjectCache CacheBackingStore { get; private set; }

        public void SetCacheBackingStore(ObjectCache objectCache)
        {
            CacheBackingStore = objectCache;
        }
    }
}
