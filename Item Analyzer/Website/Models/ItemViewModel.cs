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
	}
}