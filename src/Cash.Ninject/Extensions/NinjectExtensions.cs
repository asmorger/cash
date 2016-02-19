using Cash.Ninject.Interceptors;
using Ninject.Extensions.Interception.Advice.Syntax;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Syntax;

namespace Cash.Ninject.Extensions
{
    public static class NinjectExtensions
    {
        public static IAdviceOrderSyntax WithDefaultCache<T>(this IBindingNamedWithOrOnSyntax<T> bindingSyntax)
        {
            return bindingSyntax.Intercept().With<NinjectCachingInterceptor>();
        }
    }
}
