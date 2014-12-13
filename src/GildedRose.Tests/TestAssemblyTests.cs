using System;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class InitialRequirements : TestBase
    {
        [Test]
        public void QualityDecreasesEveryDay()
        {
            foreach (var item in TestData.Where(TheItem.QualityReducesWithAge))
            {
                RepeatUntil(
                    item,
                    x =>
                    {
                        var start = x.Quality;
                        AgeItem(x);
                        Assert.Less(x.Quality, start, String.Format("item '{0}", x.Name));
                    },
                    x => x.SellIn == -1);
            }
        }

        [TestCase(10, 6, 2)]
        [TestCase(5, 0, 3)]
        public void BackstagePassesQualityIncreasesAtDifferentRates(int sellInRangeStart, int sellInRangeEnd, int expectedQualityIncrease)
        {
            var backstagePasses = TestData.Item(TheItem.IsBackstagePass);

            AgeUntil(backstagePasses, x => x.SellIn == sellInRangeStart);
            RepeatUntil(backstagePasses,
                x =>
                {
                    var initial = x.Quality;
                    AgeItem(x);
                    Assert.AreEqual(expectedQualityIncrease, x.Quality - initial);
                },
                x => x.SellIn == sellInRangeEnd);
        }

        [Test]
        public void BackstagePassesQualityIsZeroAfterExpiry()
        {
            var backstagePasses = TestData.Item(TheItem.IsBackstagePass);
            AgeUntil(backstagePasses, TheItem.HasExpired);
            Assert.AreEqual(0, backstagePasses.Quality);
        }

        [Test]
        public void QualityDegradesTwiceAsFastAfterExpiry()
        {
            foreach (var item in TestData.Where(TheItem.QualityReducesWithAge))
            {
                var initialReduction = AgeAndGetQualityReduction(item);
                AgeUntil(item, TheItem.IsOnExpiryDay);
                var reductionAfterExpiry = AgeAndGetQualityReduction(item);

                Assert.AreEqual(initialReduction*2, reductionAfterExpiry, String.Format("item '{0}", item.Name));
            }
        }

        [Test]
        public void QualityIsNeverNegative()
        {
            foreach (var item in TestData.Where(TheItem.QualityReducesWithAge))
            {
                AgeUntil(item, x => x.Quality == 0);
                Assert.That(item.Quality >= 0, String.Format("item '{0}", item.Name));
            }
        }

        [Test]
        public void QualityNeverExceedsFifty()
        {
            foreach (var item in TestData.Except(TheItem.IsSulfuras))
            {
                AgeUntil(item, x => x.SellIn == -500);
                Assert.That(item.Quality <= 50, String.Format("item '{0}", item.Name));
            }
        }

        [Test]
        public void AgedBrieQualityIncreasesWithAge()
        {
            var item = TestData.Item(TheItem.IsAgedBrie);
            RepeatUntil(
                item,
                x =>
                {
                    var start = x.Quality;
                    AgeItem(x);
                    Assert.Greater(x.Quality, start, String.Format("item '{0}", x.Name));
                },
                x => x.Quality >= 50);
        }

        [Test]
        public void SulphurasQualityNeverReduces()
        {
            var initial = TestData.Item(TheItem.IsSulfuras);
            var aged = TestData.Item(TheItem.IsSulfuras);
            AgeItem(aged);

            Assert.AreEqual(initial.Quality, aged.Quality);
        }

        [Test]
        public void SulphurasNeverExpires()
        {
            var initial = TestData.Item(TheItem.IsSulfuras);
            var aged = TestData.Item(TheItem.IsSulfuras);
            AgeItem(aged);

            Assert.AreEqual(initial.SellIn, aged.SellIn);
        }
    }
}