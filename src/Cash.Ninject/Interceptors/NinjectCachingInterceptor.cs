using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using Cash.Core.Interceptors;
using Cash.Core.Services;
using Ninject.Extensions.Interception;

namespace Cash.Ninject.Interceptors
{
    public class NinjectCachingInterceptor : BaseCachingInterceptor, IInterceptor
    {
        private IInvocation _invocation;

        public NinjectCachingInterceptor(ObjectCache cache, ICacheKeyGenerationService cacheKeyGenerationService) 
            : base(cache, cacheKeyGenerationService)
        {
        }

        public void Intercept(IInvocation invocation)
        {
            _invocation = invocation;

            ExecuteInterceptionLogic();
        }

        protected override MethodInfo GetMethodInfoFromInterceptor()
        {
            var targetType = _invocation.Request.Target.GetType();
            var parameters = _invocation.Request.Method.GetParameters();
            var argTypes = parameters.Any() ? parameters.Select(p => p.ParameterType).ToArray() : new Type[] { };
            var targetMethod = targetType.GetMethod(_invocation.Request.Method.Name, argTypes);

            return targetMethod;
        }

        protected override object[] GetArgumentsFromInterceptor()
        {
            var output = _invocation.Request.Arguments;
            return output;
        }

        protected override void SetIntercetporReturnValue(object value)
        {
            _invocation.ReturnValue = value;
        }

        protected override void InterceptorProceed()
        {
            _invocation.Proceed();
        }

        protected override object GetInterceptorReturnValue()
        {
            var output = _invocation.ReturnValue;
            return output;
        }
    }
}
