// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System.Reflection;

namespace Cash.Core.Services
{
    public interface ICacheKeyGenerationService
    {
        string GetMethodCacheKey(MethodInfo method);

        string GetArgumentsCacheKey(object[] arguments);
    }
}