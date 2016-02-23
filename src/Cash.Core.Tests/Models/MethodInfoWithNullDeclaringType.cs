// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace Cash.Core.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class MethodInfoWithNullDeclaringType : MethodInfo
    {
        public override ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        public override string Name { get; }

        public override Type DeclaringType => null;

        public override Type ReflectedType { get; }

        public override RuntimeMethodHandle MethodHandle { get; }

        public override MethodAttributes Attributes { get; }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override object Invoke(
            object obj,
            BindingFlags invokeAttr,
            Binder binder,
            object[] parameters,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}