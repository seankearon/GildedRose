using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GildedRose.Console;

namespace GildedRose.Tests
{
    public class TestData : IEnumerable<Item>
    {
        public TestData()
        {
            _program = new Program();


            AgedBrie = _program.Items.Single(x => x.Name == "Aged Brie");
            BackstagePasses = _program.Items.Single(x => x.Name.StartsWith("Backstage passes"));
            Sulphuras = _program.Items.Single(x => x.Name.StartsWith("Sulfuras"));
        }

        private readonly Program _program;

        public Item Item(Item item)
        {
            return _program.Items.Single(x => x.Name == item.Name);
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _program.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Item AgedBrie { get; set; }
        private Item BackstagePasses { get; set; }
        public Item Sulphuras { get; set; }
    }
}