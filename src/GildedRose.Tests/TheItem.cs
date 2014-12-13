using GildedRose.Console;

namespace GildedRose.Tests
{
    public static class TheItem
    {
        public static bool IsOnExpiryDay(Item item)
        {
            return item.SellIn == 0;
        }

        public static bool HasExpired(Item item)
        {
            return item.SellIn < 0;
        }

        public static bool IsConjured(Item item)
        {
            return item.Name == "Conjured Mana Cake";
        }

        public static bool IsBackstagePass(Item item)
        {
            return item.Name == "Backstage passes to a TAFKAL80ETC concert";
        }

        public static bool IsAgedBrie(Item item)
        {
            return item.Name == "Aged Brie";
        }

        public static bool IsSulfuras(Item item)
        {
            return item.Name == "Sulfuras, Hand of Ragnaros";
        }

        public static bool QualityImprovesWithAge(Item item)
        {
            return IsAgedBrie(item) || IsBackstagePass(item);
        }

        public static bool QualityReducesWithAge(Item item)
        {
            return DoesAge(item) && !QualityIsConstantWithAge(item) && !QualityImprovesWithAge(item);
        }

        public static bool QualityIsConstantWithAge(Item item)
        {
            return IsSulfuras(item);
        }

        public static bool DoesNotAge(Item item)
        {
            return IsSulfuras(item);
        }

        public static bool DoesAge(Item item)
        {
            return !DoesNotAge(item);
        }
    }
}