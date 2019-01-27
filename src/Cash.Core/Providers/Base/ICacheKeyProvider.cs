// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Cash.Core.Providers.Base
{
    public interface ICacheKeyProvider
    {
        /// <summary>
        /// Checks to see if this Parameter Broker is valid for the given parameter.
        /// </summary>
        /// <param name="parameter">The method parameter</param>
        /// <returns></returns>
        bool IsValid(object parameter);

        /// <summary>
        /// Gets the value representation of the parameter
        /// </summary>
        /// <param name="parameter">The method parameter</param>
        /// <returns></returns>
        string GetValueRepresentation(object parameter);

        /// <summary>
        /// Gets the type name representation of the parameter
        /// </summary>
        /// <param name="parameter">The method parameter</param>
        /// <returns></returns>
        string GetTypeNameRepresentation(object parameter);
    }
}
