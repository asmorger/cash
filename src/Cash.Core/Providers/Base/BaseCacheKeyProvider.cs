// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Cash.Core.Enums;

namespace Cash.Core.Providers.Base
{
    public abstract class BaseCacheKeyProvider : ICacheKeyProvider
    {
        private const string ArgumentNameValueDelimiter = "::";

        public abstract CacheKeyProviderExecutionOrder ExecutionOrder { get; }
        public abstract bool IsValid(object parameter);
        public abstract string GetValueRepresentation(object parameter);
        public abstract string GetTypeNameRepresentation(object parameter);

        public string GetKey(object parameter)
        {
            var name = GetTypeNameRepresentation(parameter);
            var value = GetValueRepresentation(parameter);

            var output = $"{name}{ArgumentNameValueDelimiter}{value}";
            return output;
        }
    }
}
