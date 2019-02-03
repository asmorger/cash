// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;

namespace Cash.Core.Interceptors
{
    public class CachingInterceptor : BaseCachingInterceptor, IInterceptor
    {
        private IInvocation _invocation;

        public CachingInterceptor(IMemoryCache cache, ICacheKeyGenerator cacheKeyGenerator)
            : base(cache, cacheKeyGenerator)
        {
        }

        public void Intercept(IInvocation invocation)
        {
            _invocation = invocation;

            ExecuteInterceptionLogic();
        }

        protected override MethodInfo GetMethodInfoFromInterceptor()
        {
            var method = _invocation.GetConcreteMethodInvocationTarget();
            return method;
        }

        protected override object[] GetArgumentsFromInterceptor()
        {
            var arguments = _invocation.Arguments;
            return arguments;
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