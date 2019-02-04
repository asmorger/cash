// Copyright (c) Andrew Morger. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable UnusedVariable

namespace Cash.Core.Tests.Attributes
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CacheAttributeTests
    {
        public CacheAttribute CacheAttribute { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            CacheAttribute = new CacheAttribute();
        }

        [TestMethod]
        public void Ctor_DefaultsThePriority_ToNormal()
        {
            var attribute = new CacheAttribute();

            Assert.AreEqual(CacheItemPriority.Normal, attribute.Priority);
        }

        [TestMethod]
        public void Ctor_SetsThePriority_ToWhatIsDefined()
        {
            var attribute = new CacheAttribute(CacheItemPriority.High);

            Assert.AreEqual(CacheItemPriority.High, attribute.Priority);
        }
        
        [TestMethod]
        public void Ctor_SetsTheDuration_ToWhatIsDefined()
        {
            const double duration = 20;
            const TimeMeasure measure = TimeMeasure.Minutes;
            
            var attribute = new CacheAttribute(duration, measure);

            Assert.AreEqual(duration, attribute.Duration);
        }
        
        [TestMethod]
        public void Ctor_SetsTheTimeMeasure_ToWhatIsDefined()
        {
            const double duration = 20;
            const TimeMeasure measure = TimeMeasure.Minutes;
            
            var attribute = new CacheAttribute(duration, measure);

            Assert.AreEqual(measure, attribute.Measure);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Ctor_ThrowsAnException_WhenTheDurationIsLessThanZero()
        {
            var attribute = new CacheAttribute(-10, TimeMeasure.Minutes);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Ctor_ThrowsAnException_WhenTheDurationIsZero()
        {
            var attribute = new CacheAttribute(0, TimeMeasure.Minutes);
        }

        [TestMethod]
        public void GetCacheEntryOptions_ReturnsAnObject()
        {
            var result = CacheAttribute.GetCacheEntryOptions();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCacheEntryOptions_SetsThePriority()
        {
            var result = new CacheAttribute(CacheItemPriority.High).GetCacheEntryOptions();

            Assert.AreEqual(CacheItemPriority.High, result.Priority);
        }
        
        [TestMethod]
        public void GetCacheEntryOptions_SetsTheSlidingExpirationInHours_WhenTheMeasureIsHours()
        {
            const double duration = 20;
            const TimeMeasure measure = TimeMeasure.Hours;
            
            var attribute = new CacheAttribute(duration, measure);
            var result = attribute.GetCacheEntryOptions();

            Assert.AreEqual(TimeSpan.FromHours(duration), result.SlidingExpiration);
        }
        
        [TestMethod]
        public void GetCacheEntryOptions_SetsTheSlidingExpirationInMinutes_WhenTheMeasureIsMinutes()
        {
            const double duration = 20;
            const TimeMeasure measure = TimeMeasure.Minutes;
            
            var attribute = new CacheAttribute(duration, measure);
            var result = attribute.GetCacheEntryOptions();

            Assert.AreEqual(TimeSpan.FromMinutes(duration), result.SlidingExpiration);
        }
        
        [TestMethod]
        public void GetCacheEntryOptions_SetsTheSlidingExpirationInSeconds_WhenTheMeasureIsSeconds()
        {
            const double duration = 20;
            const TimeMeasure measure = TimeMeasure.Seconds;
            
            var attribute = new CacheAttribute(duration, measure);
            var result = attribute.GetCacheEntryOptions();

            Assert.AreEqual(TimeSpan.FromSeconds(duration), result.SlidingExpiration);
        }
        
        [TestMethod]
        public void GetCacheEntryOptions_DefaultsTheSlidingExpirationToMinutes_WhenTheMeasureIsInvalid()
        {
            const double duration = 20;
            const TimeMeasure measure = (TimeMeasure)20;
            
            var attribute = new CacheAttribute(duration, measure);
            var result = attribute.GetCacheEntryOptions();

            Assert.AreEqual(TimeSpan.FromMinutes(duration), result.SlidingExpiration);
        }
        
        [TestMethod]
        public void GetCacheEntryOptions_SetsThePriorityToNormal_WhenSlidingExpirationIsDefined()
        {
            const double duration = 20;
            const TimeMeasure measure = (TimeMeasure)20;
            
            var attribute = new CacheAttribute(duration, measure);
            var result = attribute.GetCacheEntryOptions();

            Assert.AreEqual(CacheItemPriority.Normal, result.Priority);
        }
    }
}