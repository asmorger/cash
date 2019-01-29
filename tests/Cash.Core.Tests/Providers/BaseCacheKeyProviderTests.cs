using System.Linq;

using Cash.Core.Providers.Base;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cash.Core.Tests.Providers
{
    public abstract class BaseCacheKeyProviderTests<TCacheKeyProvider>
        where TCacheKeyProvider : ICacheKeyProvider, new()
    {
        protected ICacheKeyProvider CacheKeyProvider;

        [TestInitialize]
        public void Initialize()
        {
            CacheKeyProvider = new TCacheKeyProvider();
        }

        public abstract object[] GetSuccessArguments();

        public abstract object[] GetFailureArguments();

        [TestMethod]
        public void IsValid_IdentifiesCorrectly_WhenInvocationIsValid()
        {
            var args = GetSuccessArguments();
            var results = args.Select(x => CacheKeyProvider.IsValid(x));
            var isIdentified = results.All(x => x == true);

            Assert.IsTrue(isIdentified);
        }

        [TestMethod]
        public void IsValid_IdentifiesCorrectly_WhenInvocationAreInvalid()
        {
            var args = GetFailureArguments();
            var results = args.Select(x => CacheKeyProvider.IsValid(x));
            var isIdentified = results.All(x => x == false);

            Assert.IsTrue(isIdentified);
        }

        /*
        [TestMethod]
        public void GetKey_Generates_TheKeyWeAreExpecting()
        {
            const string ArgumentNameValueDelimiter = "::";

            var argument = GetSuccessArguments().First();

            var name = CacheKeyProvider.GetTypeNameRepresentation(argument);
            var value = CacheKeyProvider.GetValueRepresentation(argument);
            var expected = $"{name}{ArgumentNameValueDelimiter}{value}";

            var result = CacheKeyProvider.GetKey(argument);

            Assert.AreEqual(expected, result);
        }
        */
    }
}
