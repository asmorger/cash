// // Copyright (c) Andrew Morger. All rights reserved.
// // Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System.Runtime.Caching;

using Cash.Core.Services;

namespace Cash.Core
{
    public class CashContext
    {
        private static volatile CashContext instance;

        private static readonly object CreationLock = new object();

        public ObjectCache CacheBackingStore { get; private set; }

        public ICacheKeyRegistrationService RegistrationService { get; private set; }

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

        public void SetCacheBackingStore(ObjectCache objectCache)
        {
            CacheBackingStore = objectCache;
        }
    }
}