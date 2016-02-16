// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the GNU General Public License, Version 3.0. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;

using Cash.Core.Attributes;

namespace Cash.Core.Tests.Models
{
    [ExcludeFromCodeCoverage]
    public class TestModelDefinition
    {
        public int Id { get; set; }

        public void TestMethod_NoParameters()
        {
        }

        public void TestMethod_OneSimpleParameter(int x)
        {
        }

        public void TestMethod_TwoSimpleParameters(double height, int age)
        {
        }

        [Cache]
        public void TestMethod_WithCacheAttribute()
        {
        }

        public void TestMethod_WithNoCacheAttribute()
        {
        }
    }
}