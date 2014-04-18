using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Website.JSonProxies
{
	public class StashTab
	{
		[JsonProperty("numTabs")]
		public int NumberOfTabs { get; set; }

		public IEnumerable<Item> Items { get; set; }
		public IEnumerable<Tab> Tabs { get; set; }
	}

	public class Tab
	{
		[JsonProperty("i")]
		public int Index { get; set; }

		[JsonProperty("n")]
		public string Name { get; set; }
	}

	public class Item
	{
		public string Name { get; set; }
		public IEnumerable<string> ExplicitMods { get; set; }
		public string TypeLine { get; set; }
		public IEnumerable<ItemProperty> Properties { get; set; }
		public string Icon { get; set; }
		public string InventoryId { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public IEnumerable<string> ImplicitMods { get; set; }
	}

	public class ItemProperty
	{
		public string Name { get; set; }
		public IEnumerable<IEnumerable<object>> Values { get; set; }
	}
}