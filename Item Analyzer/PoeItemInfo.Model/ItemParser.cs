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
					InventoryId = item.inventoryId,
					X = item.x,
					Y = item.y,
				},
				SocketedItems = item.socketedItems != null ? item.socketedItems.Select(Parse) : new Item[0],
				ItemType = itemTypeParser.Parse(item),
			};
		}

		public IEnumerable<Item> Parse(Stash stash)
		{
			return stash.items.Select(Parse);
		}
	}
}