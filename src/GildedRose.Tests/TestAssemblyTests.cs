using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GildedRose.Console;
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
        public void QualityDegradesEachDay()
        {
            var dayZero = new TestData();
            var dayOne = new TestData().AgeBy(1);

            foreach (var original in dayZero.ItemsThatAge)
            {
                var aged = dayOne.MatchingItem(original);
                Assert.That(original.Quality - aged.Quality == 1, string.Format("item '{0}", original.Name));
            }
        }

        [Test]
        public void AfterTheSellByDateQualityDegradesTwiceAsFast()
        {
            var control = new TestData();
            foreach (var item in control.ItemsWhoseQualityDecreasesNormallyWithAge)
            {
                var dayZero = new TestData();
                var dayOne = new TestData().AgeBy(1);
                var dayOneDelta = dayZero.MatchingItem(item).Quality - dayOne.MatchingItem(item).Quality;

                var expiredZero = new TestData().AgeUntilMatchingItemIsAtSellDate(item);
                var expiredOne = new TestData().AgeUntilMatchingItemIsAtSellDate(item).AgeBy(1);
                var expiredDelta = expiredZero.MatchingItem(item).Quality - expiredOne.MatchingItem(item).Quality;

                Assert.AreEqual(expiredDelta, 2 * dayOneDelta, string.Format("item '{0}", item.Name));
            }
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


    public class TestData: IEnumerable<Item>
    {
        private bool ItemAges(Item item)
        {
            return item != Sulphuras; // Never ages;
        }

        private bool ItemAgesNormally(Item item)
        {
            return
                item != Sulphuras // Never ages.
                && item != BackstagePasses // Is zero after expiry.
                ;
        }

        private bool ItemQualityDecreasesNormallyWithAge(Item item)
        {
            return
                item != Sulphuras // Never ages.
                && item != BackstagePasses // Is zero after expiry.
                ;
        }

        private bool ItemQualityNeverDecreases(Item item)
        {
            return item == AgedBrie || item == Sulphuras;
        }

        public Item AgedBrie { get; set; }
        public Item BackstagePasses { get; set; }
        public Item Sulphuras { get; set; }
        public IEnumerable<Item> Items { get; private set; }
        public IEnumerable<Item> ItemsWhoseQualityDecreasesNormallyWithAge { get; private set; }
        public IEnumerable<Item> ItemsThatAge { get; private set; }
        public IEnumerable<Item> ItemsWhoseQualityNeverDecreases { get; private set; }
        public IEnumerable<Item> ItemsWhoseQualityDecreases { get; private set; }
        
        public IEnumerable<Item> ConjuredItems { get; private set; }

        private bool IsConjured(Item item)
        {
            return item.Name.StartsWith("Conjured");
        }

        public Item MatchingItem(Item item)
        {
            return Items.Single(x => x.Name == item.Name);
        }

        public TestData SetSellIn(int sellIn)
        {
            foreach (var item in Items)
            {
                item.SellIn = 0;
            }
            return this;
        }

        public TestData AgeUntilMatchingItemIsAtSellDate(Item item)
        {
            var matchingItem = MatchingItem(item);
            var sellIn = matchingItem.SellIn;

            while (matchingItem.SellIn > 0)
            {
                AgeBy(1);
                if (matchingItem.SellIn >= sellIn) throw new Exception(string.Format("This items sell in does not decrease: '{0}'.", item.Name));
            }
            return this;
        }

        public TestData AgeBy(int days)
        {
            for (var i = 1; i <= days; i++)
            {
                _program.UpdateQuality();
            }
            return this;
        }

        private readonly Program _program;

        public TestData()
        {
            _program = new Program();

            Items = _program.Items;
            
            AgedBrie = _program.Items.Single(x => x.Name == "Aged Brie");
            BackstagePasses = _program.Items.Single(x => x.Name.StartsWith("Backstage passes"));
            Sulphuras = _program.Items.Single(x => x.Name.StartsWith("Sulfuras"));

            ConjuredItems = _program.Items.Where(IsConjured).ToArray();
            ItemsWhoseQualityDecreasesNormallyWithAge = _program.Items.Where(ItemQualityDecreasesNormallyWithAge).ToArray();
            
            ItemsThatAge = _program.Items.Where(ItemAges).ToArray();
            
            ItemsWhoseQualityNeverDecreases = _program.Items.Where(ItemQualityNeverDecreases).ToArray();
            ItemsWhoseQualityDecreases = Items.Except(ItemsWhoseQualityNeverDecreases).ToArray();
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}