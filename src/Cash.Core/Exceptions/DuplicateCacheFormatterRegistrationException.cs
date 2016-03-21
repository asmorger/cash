// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Cash.Core.Exceptions
{
    public class DuplicateCacheFormatterRegistrationException : Exception
    {
        public DuplicateCacheFormatterRegistrationException(Type type)
            : base($"A registration for the type {type.Name} has already been registered with the system.")
        {
        }
    }
}