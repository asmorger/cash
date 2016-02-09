    public static class AutofacHelper
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> WithDefaultCache<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
        {
            return registration.EnableInterfaceInterceptors().InterceptedBy(typeof (CacheAttributeInterceptor));
        }
    }