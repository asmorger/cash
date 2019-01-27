using System.Diagnostics.CodeAnalysis;
using Cash.Core.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class NullCacheKeyProviderTests : BaseCacheKeyProviderTests<NullCacheKeyProvider>
    {
        public override object[] GetSuccessArguments() => new object[] { null };

        public override object[] GetFailureArguments() => new object[] { 1, "test", new Models.TestModelDefinition() };
    }
}
