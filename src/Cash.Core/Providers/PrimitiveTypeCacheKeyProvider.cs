// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Cash.Core.Providers.Base;

namespace Cash.Core.Providers
{
    public class PrimitiveTypeCacheKeyProvider : BaseCacheKeyProvider
    {
        public override bool IsValid(object parameter)
        {
            var type = parameter.GetType();
            var output = type.IsPrimitive || type == typeof(string);
            return output;
        }

        public override string GetValueRepresentation(object parameter)
        {
            var output = parameter.ToString();
            return output;
        }

        public override string GetTypeNameRepresentation(object parameter)
        {
            var type = parameter.GetType();
            var output = type.Name;

            return output;
        }
    }
}
