// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Reflection;

namespace Cash.Core.Services
{
    public interface ICacheKeyGenerationService
    {
        string GetMethodCacheKey(MethodInfo method);

        string GetArgumentsCacheKey(object[] arguments);

        string GetCacheKey(MethodInfo method, object[] arguments);
    }
}