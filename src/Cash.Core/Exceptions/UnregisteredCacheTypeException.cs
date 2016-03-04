// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Cash.Core.Exceptions
{
    public class UnregisteredCacheTypeException : Exception
    {
        public UnregisteredCacheTypeException(Type type)
            : base($"The type '{type.FullName}' has not been registered as a cache type.")
        {
        }
    }
}