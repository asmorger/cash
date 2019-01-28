// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;

using Cash.Core.Providers.Base;

namespace Cash.Core.Providers
{
    public class EumCacheKeyProvider : BaseCacheKeyProvider
    {
        public override bool IsValid(object parameter)
        {
            var output = parameter is Enum;
            return output;
        }

        public override string GetValueRepresentation(object parameter)
        {
            // pull the value out of the underlying type
            var item = Convert.ChangeType(parameter, Enum.GetUnderlyingType(parameter.GetType()));

            var output = item.ToString();
            return output;
        }

        public override string GetTypeNameRepresentation(object parameter)
        {
            var type = parameter.GetType();
            var key = $"Enum[{type.Name}]";

            return key;
        }
    }
}
