// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Cash.Core.Providers.Base;

namespace Cash.Core.Providers
{
    public sealed class NullCacheKeyProvider : BaseCacheKeyProvider
    {
        public override bool IsValid(object parameter)
        {
            var output = parameter == null;
            return output;
        }

        public override string GetValueRepresentation(object parameter) => "[NULL]";

        public override string GetTypeNameRepresentation(object parameter) => "[UnknownType]";
    }
}
