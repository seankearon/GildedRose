using System;
using System.Linq;
using NUnit.Framework;

namespace GildedRose.Tests
{
    public class FutureRequirements: TestBase
    {
        [Test]
        public void ConjuredItemsDecreaseInQualityAtTwiceTheRate()
        {
            var normalItem = TestData.Where(TheItem.QualityReducesWithAge).First();
            var normalReduction = AgeAndGetQualityReduction(normalItem); // Expect the reduction to be 1, but it's never actually stated.

            foreach (var conjured in TestData.Where(TheItem.IsConjured))
            {
                var conjuredReduction = AgeAndGetQualityReduction(conjured);
                Assert.AreEqual(normalReduction * 2, conjuredReduction, String.Format("item = '{0}'", conjured.Name));
            }
        }
    }
}