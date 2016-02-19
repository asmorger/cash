using System;

using Cash.Core.Attributes;

namespace Cash.Autofac.Sample.Web.Services
{
    public class RandomDataService : IRandomDataService
    {
        private readonly Random _random;

        public RandomDataService()
        {
            _random = new Random(500);
        }

        [Cache]
        public int GetCachedRandomNumber()
        {
            var nextNumber = _random.Next();
            return nextNumber;
        }

        public int GetNonCachedRandomNumber()
        {
            var nextNumber = _random.Next();
            return nextNumber;
        }
    }
}
