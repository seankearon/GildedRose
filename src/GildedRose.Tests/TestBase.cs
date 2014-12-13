using System;
using GildedRose.Console;
using NUnit.Framework;
namespace GildedRose.Tests
{
    public class TestBase
    {
        public static void AgeItem(Item item)
        {
            var program = new Program();
            program.Items.Clear();
            program.Items.Add(item);
            program.UpdateQuality();
        }

        protected void RepeatUntil(Item item, Action<Item> action, Predicate<Item> condition)
        {
            do
            {
                action(item);
            } while (!condition(item));
        }

        protected void AgeUntil(Item item, Predicate<Item> condition)
        {
            RepeatUntil(item, AgeItem, condition);
        }

        protected int AgeAndGetQualityReduction(Item item)
        {
            var initial = item.Quality;
            AgeItem(item);
            return initial - item.Quality;
        }
    }
}