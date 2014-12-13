// Content copyright © Atlantic Business Systems Ltd 2014.  All Rights Reserved.

using System;
using System.Linq;
using GildedRose.Console;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class InitialRequirement
    {
        public static void AgeItem(Item item)
        {
            var program = new Program();
            program.Items.Clear();
            program.Items.Add(item);
            program.UpdateQuality();
        }

        bool ExpiryDay(Item item)
        {
            return item.SellIn == 0;
        }

        bool Expired(Item item)
        {
            return item.SellIn < 0;
        }

        private bool IsBackstagePass(Item item)
        {
            return item.Name == "Backstage passes to a TAFKAL80ETC concert";
        }

        private bool IsAgedBrie(Item item)
        {
            return item.Name == "Aged Brie";
        }

        private bool IsSulfuras(Item item)
        {
            return item.Name == "Sulfuras, Hand of Ragnaros";
        }

        private bool IsConjured(Item item)
        {
            return item.Name == "Conjured Mana Cake";
        }

        private bool QualityImprovesWithAge(Item item)
        {
            return IsAgedBrie(item) || IsBackstagePass(item);
        }

        private bool QualityReducesWithAge(Item item)
        {
            return DoesAge(item) && !QualityIsConstantWithAge(item) && !QualityImprovesWithAge(item);
        }

        private bool QualityIsConstantWithAge(Item item)
        {
            return IsSulfuras(item);
        }

        private bool DoesNotAge(Item item)
        {
            return IsSulfuras(item);
        }

        private bool DoesAge(Item item)
        {
            return !DoesNotAge(item);
        }

        private void RepeatUntil(Item item, Action<Item> action, Predicate<Item> condition)
        {
            do
            {
                action(item);
            } while (!condition(item));
        }

        [Test]
        public void QualityDecrasesEveryDay()
        {
            var data = new TestData();
            foreach (var item in data.Where(QualityReducesWithAge))
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
        public void BackstagePassesQualityIncreasesAtDifferentRatesCloserToTheConcert(int sellInRangeStart, int sellInRangeEnd, int expectedQualityIncrease)
        {
            var backstagePasses = new TestData().Single(IsBackstagePass);

            RepeatUntil(backstagePasses, AgeItem, x => x.SellIn == sellInRangeStart);
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
        public void BackstagePassesQualityIsZeroAfterTheConcert()
        {
            var backstagePasses = new TestData().Single(IsBackstagePass);
            RepeatUntil(backstagePasses, AgeItem, Expired);
            Assert.AreEqual(0, backstagePasses.Quality);
        }

        [Test]
        public void AfterExpiryQualityDegradesTwiceAsFast()
        {
            var data = new TestData();
            foreach (var item in data.Where(QualityReducesWithAge))
            {
                var initialReduction = AgeAndGetQualityReduction(item);
                RepeatUntil(item, AgeItem, ExpiryDay);
                var reductionAfterExpiry = AgeAndGetQualityReduction(item);

                Assert.AreEqual(initialReduction * 2, reductionAfterExpiry, String.Format("item '{0}", item.Name));
            }
        }

        [Test]
        public void ItemQualityIsNeverNegative()
        {
            var data = new TestData();
            foreach (var item in data.Where(QualityReducesWithAge))
            {
                RepeatUntil(item, AgeItem, x => x.Quality == 0);
                Assert.That(item.Quality >= 0, String.Format("item '{0}", item.Name));
            }
        }

        [Test]
        public void QualityIsNeverMoreThanFifty()
        {
            var data = new TestData();
            foreach (var item in data.Except(new[] {data.Sulphuras}))
            {
                RepeatUntil(item, AgeItem, x => x.SellIn == -500);
                Assert.That(item.Quality <= 50, String.Format("item '{0}", item.Name));
            }
        }

        [Test]
        public void AgedBrieIncreasesInQualityWithAge()
        {
            var item = new TestData().AgedBrie;
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
        public void SulphurasNeverDecreasesInQuality()
        {
            var data = new TestData();
            foreach (var item in data.Except(new[] {data.Sulphuras}))
            {
                RepeatUntil(item, AgeItem, x => x.SellIn == -500);
                Assert.That(item.Quality <= 50, String.Format("item '{0}", item.Name));
            }
        }

        private int AgeAndGetQualityReduction(Item item)
        {
            var initial = item.Quality;
            AgeItem(item);
            return initial - item.Quality;
        }
    }

    public class FutureRequirements
    {
        //[Test]
        //public void ConjuredItemsDecreaseInQualityAtTwiceTheRate()
        //{
        //    var data = new TestData();

        //    var normalItem = data.First(AgesNormally);
        //    var normalReduction = AgeAndGetQualityReduction(normalItem); // Expect this to be 1, but it's never stated.

        //    foreach (var conjured in data.Where(IsConjured))
        //    {
        //        var conjuredReduction = AgeAndGetQualityReduction(conjured);
        //        Assert.AreEqual(normalReduction * 2, conjuredReduction, String.Format("item = '{0}'", conjured.Name));
        //    }
        //}
    }
}