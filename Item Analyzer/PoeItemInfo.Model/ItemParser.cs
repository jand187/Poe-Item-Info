using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PoeItemInfo.Data.Model.JSonProxy;

namespace PoeItemInfo.Model
{
	public interface IItemParser
	{
		Item Parse(Data.Model.JSonProxy.Item item);
		Item Parse(Data.Model.JSonProxy.Item item, IEnumerable<Tab> tabs);
		IEnumerable<Item> Parse(Stash stash);
	}

	public class ItemParser : IItemParser
	{
		private readonly IPropertyParser propertyParser;
		private readonly IRequirementParser requirementParser;
		private readonly IModsParser modsParser;
		private readonly IItemTypeParser itemTypeParser;

		public ItemParser(IPropertyParser propertyParser, IRequirementParser requirementParser, IModsParser modsParser, IItemTypeParser itemTypeParser)
		{
			this.propertyParser = propertyParser;
			this.requirementParser = requirementParser;
			this.modsParser = modsParser;
			this.itemTypeParser = itemTypeParser;
		}

		public Item Parse(Data.Model.JSonProxy.Item item)
		{
			return Parse(item, new Tab[0]);
		}

		public Item Parse(Data.Model.JSonProxy.Item item, IEnumerable<Tab> tabs)
		{
			return new Item
			{
				Original = item,
				Name = string.IsNullOrWhiteSpace(item.name) ? item.typeLine : item.name,
				Icon = item.icon,
				TypeLine = item.typeLine,
				Identified = item.identified,
				Corrupted = item.corrupted,
				Properties = propertyParser.Parse(item.properties),
				Requirements = requirementParser.Parse(item.requirements),
				ExplicitMods = modsParser.Parse((item.explicitMods)),
				ImplicitMods = modsParser.Parse((item.implicitMods)),
				FrameType = item.frameType,
				Location = new ItemLocation
				{
					Tab = GetTabName(item.inventoryId, tabs),
					InventoryId = item.inventoryId,
					X = item.x,
					Y = item.y,
				},
				SocketedItems = item.socketedItems != null ? item.socketedItems.Select(item1 => Parse(item1)) : new Item[0],
				ItemType = itemTypeParser.Parse(item),
			};
		}

		private string GetTabName(string inventoryId, IEnumerable<Tab> tabs)
		{
			if (string.IsNullOrWhiteSpace(inventoryId))
				return "unknown";

			var tabId = Convert.ToInt32(Regex.Replace(inventoryId, @"^[^\d)]*", "")) - 1;
			var tab = tabs.SingleOrDefault(t => t.i == tabId);
			return tab != null ? tab.n : "unknown";
		}

		public IEnumerable<Item> Parse(Stash stash)
		{
			return stash.items.Select(i => Parse(i, stash.tabs));
		}
	}
}
