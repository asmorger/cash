using System;
using System.Linq.Expressions;

namespace Cash.Core.Services
{
    public interface ICacheKeyRegistrationService
    {
        void AddTypedCacheKeyProvider<TEntity>(Expression<Func<TEntity, string>> registrationPattern);

        Expression<Func<TEntity, string>> GetTypedCacheKeyProvider<TEntity>();

        void ClearCacheKeyProviders();
    }
}
