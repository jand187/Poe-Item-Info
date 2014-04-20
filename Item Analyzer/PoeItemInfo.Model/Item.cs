using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Model
{
	public class Item
	{
		public Data.Model.JSonProxy.Item Original { get; set; }

		public string Name { get; set; }
		public string TypeLine { get; set; }
		public bool Identified { get; set; }
		public bool Corrupted { get; set; }
		public int Quality { get; set; }
		public IEnumerable<ItemProperty> Properties { get; set; }
		public IEnumerable<ItemRequirement> Requirements { get; set; }
		public IEnumerable<ItemMod> ExplicitMods { get; set; }
		public IEnumerable<ItemMod> ImplicitMods { get; set; }
		public ItemLocation Location { get; set; }
		public IEnumerable<Item> SocketedItems { get; set; }
	}

	public class ItemProperty
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class ItemRequirement
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class ItemMod
	{
		public string Name { get; set; }
	}

	public class ItemLocation
	{
		public string InventoryId { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}
}