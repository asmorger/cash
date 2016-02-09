using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Castle.Core.Internal;
using Castle.DynamicProxy;
using Purina.ProPlan.Caching.Core;
using Purina.ProPlan.Caching.Exceptions;
using Purina.ProPlan.Caching.ParameterBrokers;
namespace Purina.ProPlan.Caching.Interceptors
{
    public class CacheAttributeInterceptor : IInterceptor
    {
        private readonly ICacheService _cacheService;
        private readonly IEnumerable<IParameterBroker> _parameterBrokers;
        public CacheAttributeInterceptor(ICacheService cacheService, IEnumerable<IParameterBroker> parameterBrokers)
        {
            _cacheService = cacheService;
            _parameterBrokers = parameterBrokers;
        }
        public void Intercept(IInvocation invocation)
        {
            WriteDebugMessage("Entering Cache Attribute Interceptor");
            var method = invocation.GetConcreteMethodInvocationTarget();
            if (!method.HasAttribute<CacheAttribute>())
            {
                invocation.Proceed();
                return;
            }
            var cacheKey = GetMethodCacheKey(invocation);
            WriteDebugMessage("Cached key generated: {0}", cacheKey);
            if (string.IsNullOrEmpty(cacheKey))
            {
                throw new InvalidOperationException("Invalid caching interceptor defined for class/method.  Please check your method inputs and Autofac configuration.");
            }
            if (_cacheService.Contains(cacheKey))
            {
                WriteDebugMessage("Cached item for key [{0}] found!", cacheKey);
                invocation.ReturnValue = _cacheService.Get(cacheKey);
                return;
            }
            invocation.Proceed();
            if (invocation.ReturnValue != null)
            {
                _cacheService.Add(cacheKey, invocation.ReturnValue);
                WriteDebugMessage("Item retrieved and cached for key [{0}]", cacheKey);
            }
            else
            {
                WriteDebugMessage("Item retrieved and was NULL for key [{0}]", cacheKey);
            }
        }
        public string GetMethodCacheKey(IInvocation invocation)
        {
            /* At first I used invocation.GetConcreteMethodInvocationTarget() as the type for this key,
             * but that is returning the type that the method is declared on, not the current typed instance.
             * 
             * ex: SpeciesDataService.GetAll() was returning the type of [SpeciesDataService] not [CatDataService]
             * therefore we were getting invalid cache keys and invalid species data.
             */
            var declaringType = invocation.TargetType;
            var className = string.Concat(declaringType.Namespace, ".", declaringType.Name);
            var methodName = invocation.Method.Name;
            var typeNames = invocation.Method.ReturnType.GenericTypeArguments.Any()
                                ? string.Join(",", invocation.Method.ReturnType.GenericTypeArguments.Select(x => x.Name))
                                : string.Empty;
            var keyFormat = string.IsNullOrEmpty(typeNames) ? "{0}.{1}" : "{0}.{1}<{2}>";
            var methodKey = string.Format(keyFormat, className, methodName, typeNames);
            if (invocation.Arguments.Any())
            {
                var parametersKey = GetParametersKey(invocation);
                methodKey = string.Concat(methodKey, "||", parametersKey);
            }
            return methodKey;
        }
        private string GetParametersKey(IInvocation invocation)
        {
            var brokers = _parameterBrokers.OrderBy(x => (int) x.ProcessOrder).ToList();
            var mapping = (from arg in invocation.Arguments
                          let broker = brokers.FirstOrDefault(x => x.IsValid(arg))
                          select new
                          {
                              Argument = arg,
                              Broker = broker
                          }).ToList();
            if (mapping.Any(x => x.Broker == null))
            {
                var invalidArguments = mapping.Where(x => x.Broker == null).Select(x => x.Argument);
                throw new InvalidParameterBrokersException(invalidArguments);
            }
            var keys = mapping.Select(x => x.Broker.GetKey(x.Argument));
            var joinedKeys = string.Join("##", keys);
            return joinedKeys;
        }
        private void WriteDebugMessage(string message, params object[] args )
        {
#if DEBUG
            var formatted = string.Format(message, args);
            Debug.WriteLine(formatted);
#endif
        }
    }
}