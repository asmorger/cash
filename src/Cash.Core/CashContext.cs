using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace Cash.Core
{
    public static class CashContext
    {
        private static readonly IDictionary<Type, LambdaExpression> CacheKeyProviders = new Dictionary<Type, LambdaExpression>();

        public static ObjectCache CacheProvider { get; private set; }

        public static void SetCacheProvider(ObjectCache objectCache)
        {
            CacheProvider = objectCache;
        }
        
        public static void AddProvider<TEntity>(Expression<Func<TEntity, bool>> registrationPattern) 
        {
            // using this for inspiration: http://stackoverflow.com/questions/16678057/list-of-expressionfunct-tproperty
            CacheKeyProviders.Add(typeof(TEntity), registrationPattern);
        }
    }
}
