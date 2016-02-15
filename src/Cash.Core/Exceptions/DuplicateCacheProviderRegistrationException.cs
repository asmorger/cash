// // Copyright (c) Andrew Morger. All rights reserved.
// // Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System;

namespace Cash.Core.Exceptions
{
    public class DuplicateCacheProviderRegistrationException : Exception
    {
        public DuplicateCacheProviderRegistrationException(Type type)
            : base($"A registration for the type {type.Name} has already been registered with the system.")
        {
        }
    }
}