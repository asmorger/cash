// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Cash.Core.Providers.Base
{
    public abstract class BaseCacheKeyProvider : ICacheKeyProvider
    {
        public abstract bool IsValid(object parameter);
        public abstract string GetValueRepresentation(object parameter);
        public abstract string GetTypeNameRepresentation(object parameter);
    }
}
