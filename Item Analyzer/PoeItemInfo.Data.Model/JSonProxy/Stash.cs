using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Data.Model.JSonProxy
{
	public class Stash
	{
		public int numTabs { get; set; }
		public IEnumerable<Item> items { get; set; }
		public IEnumerable<Tab> tabs { get; set; }
	}
}