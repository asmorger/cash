﻿using Cash.Core.Enums;

namespace Cash.Core.Providers.Base
{
    internal interface ICacheKeyProvider
    {
        /// <summary>
        /// Gets the execution order of the Cache Key Providers.  This is to allow specific implementations to run before generic ones.
        /// </summary>
        CacheKeyProviderExecutionOrder ExecutionOrder { get; }

        /// <summary>
        /// Checks to see if this Parameter Broker is valid for the given parameter.
        /// </summary>
        /// <param name="parameter">The method parameter</param>
        /// <returns></returns>
        bool IsValid(object parameter);

        /// <summary>
        /// Generates a unique key for the parameter
        /// </summary>
        /// <param name="parameter">The method parameter</param>
        /// <returns></returns>
        string GetKey(object parameter);
    }
}
