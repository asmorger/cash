// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;

using Cash.Core.Enums;
using Cash.Core.Providers.Base;

namespace Cash.Core.Providers
{
    public class EumCacheKeyProvider : BaseCacheKeyProvider
    {
        public override CacheKeyProviderExecutionOrder ExecutionOrder => CacheKeyProviderExecutionOrder.Enum;

        public override bool IsValid(object parameter)
        {
            var output = parameter is Enum;
            return output;
        }

        public override string GetValueRepresentation(object parameter)
        {
            // since it's an enum we can safely cast it to an int
            var item = (int)parameter;

            var output = item.ToString(CultureInfo.InvariantCulture);
            return output;
        }

        public override string GetTypeNameRepresentation(object parameter)
        {
            var type = parameter.GetType();
            var key = $"Enum[{type}]";

            return key;
        }
    }
}
