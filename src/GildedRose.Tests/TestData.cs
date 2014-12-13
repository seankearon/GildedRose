using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.Console;

namespace GildedRose.Tests
{
    public static class TestData
    {
        public static Item Item(Predicate<Item> condition)
        {
            return new Program().Items.Single(x => condition(x));
        }

        public static IEnumerable<Item> Where(Predicate<Item> condition)
        {
            return new Program().Items.Where(x => condition(x)).ToArray();
        }

        public static IEnumerable<Item> Except(Predicate<Item> condition)
        {
            return new Program().Items.Where(x => !condition(x));
        }
    }
}