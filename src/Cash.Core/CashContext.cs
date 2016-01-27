using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Caching;
using Cash.Core.Exceptions;

namespace Cash.Core
{
    public class CashContext
    {
        private static volatile CashContext instance;
        private static readonly object CreationLock = new object();

        private readonly IDictionary<Type, LambdaExpression> _cacheKeyProviders = new Dictionary<Type, LambdaExpression>();

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
            
        }

        public ObjectCache CacheProvider { get; private set; }

        public void ClearCacheProviders()
        {
            _cacheKeyProviders.Clear();
        }

        public void SetCacheProvider(ObjectCache objectCache)
        {
            CacheProvider = objectCache;
        }
        
        public void AddProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern)
        {
            var targetType = typeof (TEntity);

            if (_cacheKeyProviders.ContainsKey(targetType))
            {
                throw new DuplicateCacheProviderRegistrationException(targetType);
            }

            // using this for inspiration: http://stackoverflow.com/questions/16678057/list-of-expressionfunct-tproperty
            _cacheKeyProviders.Add(targetType, registrationPattern);
        }

        public Expression<Func<TEntity, string>> GetProvider<TEntity>()
        {
            var targetType = typeof (TEntity);

            if (_cacheKeyProviders.ContainsKey(targetType))
            {
                var provider = _cacheKeyProviders[targetType];
                return (Expression<Func<TEntity, string>>)provider;
            }

            return null;
        }
    }
}
