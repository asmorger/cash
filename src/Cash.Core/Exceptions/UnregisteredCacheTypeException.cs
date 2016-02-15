// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

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