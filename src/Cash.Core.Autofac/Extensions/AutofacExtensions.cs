using Autofac.Builder;
using Autofac.Extras.DynamicProxy2;
using Cash.Core.Interceptors;

namespace Cash.Core.Autofac.Extensions
{
    /// <summary>
    /// A class to house Autofac Extension Methods
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// Enables support for caching the method output based upon it's type, name, and arguments
        /// </summary>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> WithDefaultCache<TLimit, TActivatorData, TSingleRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            return registration.EnableInterfaceInterceptors().InterceptedBy(typeof(CachingInterceptor));
        }
    }
}
