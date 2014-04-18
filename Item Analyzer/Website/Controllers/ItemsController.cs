using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Website.JSonProxies;
using Website.Models;
using ItemProperty = Website.Models.ItemProperty;

namespace Website.Controllers
{
	public class ItemsController : Controller
	{
		public ActionResult Index(int page = 50)
		{
			var stashTab = GetStashTab(page);

			var model = new ItemsListViewModel
			{
				CurrentPage = page,
				ItemViewModels = GetItemViewModels(stashTab.Items, stashTab),
			};

			return View(model);
		}

		public ActionResult Search(string selectedAffix)
		{
			var firstTab = GetStashTab(0);

			var items = new List<Item>(firstTab.Items);

			for (var page = 1; page < firstTab.NumberOfTabs; page++)
			{
				var tab = GetStashTab(page);
				items.AddRange(tab.Items);
			}

			var itemViewModels = GetItemViewModels(items, firstTab);

			if (!string.IsNullOrWhiteSpace(selectedAffix))
				itemViewModels = itemViewModels
					.SelectMany(item => item.Affixes, (item, affix) => new {item, affix})
					.Where(@t => @t.affix.Name.Equals(selectedAffix))
					.OrderByDescending(@t => @t.item.Affixes.First(a => a.Name.Equals(selectedAffix)).Value)
					.Select(@t => @t.item);

			var model = new ItemsListViewModel
			{
				ItemViewModels = itemViewModels
			};


			return View(model);
		}

		private static IEnumerable<ItemViewModel> GetItemViewModels(IEnumerable<Item> items, StashTab firstTab)
		{
			var itemViewModels = items.Where(e => !string.IsNullOrWhiteSpace(e.Name)).Select(item => new ItemViewModel
			{
				Name = item.Name,
				Affixes = item.ExplicitMods.Select(Affix.Parse),
				ImplicitMods = item.ImplicitMods != null ? item.ImplicitMods.Select(Affix.Parse) : new Affix[0],
				TypeLine = item.TypeLine,
				Type = item.Properties != null ? item.Properties.First().Name : string.Empty,
				Properties = GetProperties(item),
				Icon = item.Icon,
				TabName = firstTab.Tabs.First(t => t.Index == Convert.ToInt32(item.InventoryId.Replace("Stash", string.Empty)) - 1).Name,
				Location = string.Format("{0} x {1}", item.X, item.Y)
			});
			return itemViewModels;
		}

		private static IEnumerable<ItemProperty> GetProperties(Item item)
		{
			if (item.Properties == null)
				return new ItemProperty[0];

			var itemProperties = item.Properties.Select(CreateItemProperty);

			return itemProperties;
		}

		private static ItemProperty CreateItemProperty(JSonProxies.ItemProperty p)
		{
			return new ItemProperty
			{
				Key = p.Name,
				Value = p.Values.Any() ? p.Values.First().First().ToString() : string.Empty,
				Modified = p.Values.Any() && p.Values.First().Last().ToString() == "1",
			};
		}

		private static StashTab GetStashTab(int page)
		{
			var tabDatafilePath = string.Format(@"C:\Users\janand\Documents\GitHub\Poe-Item-Info\ItemData\jand187\tab{0}.json", page);
			var contents = System.IO.File.ReadAllText(tabDatafilePath);
			var stashTab = JsonConvert.DeserializeObject<StashTab>(contents);
			return stashTab;
		}
	}

	public class AffixComparer : IComparer<Affix>
	{
		public int Compare(Affix x, Affix y)
		{
			if (x.Value == y.Value)
				return 0;

			if (x.Value < y.Value)
				return -1;

			return 1;
		}
	}
}