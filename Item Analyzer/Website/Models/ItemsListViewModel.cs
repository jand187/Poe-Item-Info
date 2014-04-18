using System.Collections.Generic;
using System.Linq;

namespace Website.Models
{
	public class ItemsListViewModel
	{
		public IEnumerable<ItemViewModel> ItemViewModels { get; set; }
		public int CurrentPage { get; set; }

		public IEnumerable<string> AffixNames
		{
			get { return ItemViewModels.SelectMany(i => i.Affixes.Select(a => a.Name)).Distinct(); }
		}

		public string SelectedAffix { get; set; }
	}
}