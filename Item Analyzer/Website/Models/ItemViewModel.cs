using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
	public class ItemViewModel
	{
		public string Name { get; set; }
		public IEnumerable<Affix> Affixes { get; set; }
		public string TypeLine { get; set; }
		public string Type { get; set; }
		public string Icon { get; set; }
		public string TabName { get; set; }
		public string Location { get; set; }
		public IEnumerable<ItemProperty> Properties { get; set; }
		public IEnumerable<Affix> ImplicitMods { get; set; }
	}

	public class ItemProperty
	{
		public string Key { get; set; }
		public string Value { get; set; }
		public bool Modified { get; set; }
	}
}