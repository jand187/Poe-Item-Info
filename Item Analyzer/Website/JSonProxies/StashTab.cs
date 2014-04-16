using System.Collections.Generic;
using System.Linq;

namespace Website.JSonProxies
{
	public class StashTab
	{
		public IEnumerable<Item> Items { get; set; }
	}

	public class Item
	{
		public string Name { get; set; }
		public IEnumerable<string> ExplicitMods { get; set; }
		public string TypeLine { get; set; }
		public IEnumerable<ItemProperty> Properties { get; set; }
		public string Icon { get; set; }
	}

	public class ItemProperty
	{
		public string Name { get; set; }
//		public IEnumerable<object> Values { get; set; }
	}
}