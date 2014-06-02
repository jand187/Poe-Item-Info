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

	public class Character
	{
		public string name { get; set; }
		public string league { get; set; }
		public int classId { get; set; }
		public string @class { get; set; }
		public int level { get; set; }
	}
}
