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
	}
}