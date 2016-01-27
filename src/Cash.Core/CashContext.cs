using System;
using System.Linq.Expressions;
using System.Runtime.Caching;
using Cash.Core.Services;

namespace Cash.Core
{
    public class CashContext
    {
        private static volatile CashContext instance;
        private static readonly object CreationLock = new object();

        private readonly ICacheKeyRegistrationService _cacheKeyRegistrationService;

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
            _cacheKeyRegistrationService = new CacheKeyRegistrationService();
        }

        public ObjectCache CacheBackingStore { get; private set; }

        public void ClearCacheKeyProviders()
        {
            _cacheKeyRegistrationService.ClearCacheKeyProviders();
        }

        public void SetCacheBackingStore(ObjectCache objectCache)
        {
            CacheBackingStore = objectCache;
        }
        
        public void AddTypedCacheKeyProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern)
        {
            _cacheKeyRegistrationService.AddTypedCacheKeyProvider(registrationPattern);
        }

        public Expression<Func<TEntity, string>> GetTypedCacheKeyProvider<TEntity>()
        {
            var output = _cacheKeyRegistrationService.GetTypedCacheKeyProvider<TEntity>();
            return output;
        }
    }
}
