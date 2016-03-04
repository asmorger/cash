// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Cash.Ninject.Interceptors;
using Ninject.Extensions.Interception.Advice.Syntax;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Syntax;

namespace Cash.Ninject.Extensions
{
    public static class NinjectExtensions
    {
        public static IAdviceOrderSyntax WithDefaultCache<T>(this IBindingNamedWithOrOnSyntax<T> bindingSyntax)
        {
            return bindingSyntax.Intercept().With<NinjectCachingInterceptor>();
        }
    }
}
