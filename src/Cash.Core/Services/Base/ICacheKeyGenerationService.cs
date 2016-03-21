// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Reflection;

namespace Cash.Core.Services
{
    public interface ICacheKeyGenerationService
    {
        string GetArgumentsCacheKey(object[] arguments);

        string GetCacheKey(MethodInfo method, object[] arguments);
    }
}