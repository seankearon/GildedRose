using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class BackstagePasses
    {
        [TestCase(10, 6, 2)]
        [TestCase(5, 0, 3)]
        public void QualityIncreasesAtDifferentRatesCloserToTheConcert(int rangeStart, int rangeEnd, int expectedIncrease)
        {
            Assert.Fail("Not implemented.");
        }
        public void QualityIsZeroAfterTheConcert()
        {
            Assert.Fail("Not implemented.");
        }
    }

    [TestFixture]
    public class GeneralTests
    {
        [Test]
        public void AfterTheSellByDateQualityDegradesTwiceAsFast()
        {
            Assert.Fail("Not implemented.");
        }

        [Test]
        public void ItemQualityIsNeverNegative()
        {
            Assert.Fail("Not implemented.");
        }

        [Test]
        public void ItemQualityIsNeverMoreThanFiftyExceptSulfuras()
        {
            Assert.Fail("Not implemented.");
        }

        [Test]
        public void AgedBrieIncreasesInQualityWithAge()
        {
            Assert.Fail("Not implemented.");
        }

        [Test]
        public void SulphurasNeverDecreasesInQuality()
        {
            Assert.Fail("Not implemented.");
        }

        [Test]
        public void ConjuredItemsDecreaseInQualityAtTwiceTheRate()
        {
            Assert.Fail("Not implemented.");
        }
    }


}