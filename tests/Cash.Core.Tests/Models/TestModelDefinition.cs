// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

        public void TestMethod_EnumerableParameter(IEnumerable<int> heights)
        {
            
        }

        public IEnumerable<int> TestMethod_EnumerableReturn()
        {
            return Enumerable.Range(0, 10);
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