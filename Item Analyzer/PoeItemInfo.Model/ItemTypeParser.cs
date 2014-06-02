using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Model
{
	public interface IItemTypeParser
	{
		ItemType Parse(Data.Model.JSonProxy.Item item);
	}

	public class ItemTypeParser : IItemTypeParser
	{
		private readonly List<BaseType> baseTypes;
		private readonly IEnumerable<ItemType> itemTypes;

		public ItemTypeParser(IEnumerable<ItemType> itemTypes)
		{
			this.itemTypes = itemTypes;
		}

		public ItemTypeParser(List<BaseType> baseTypes)
		{
			this.baseTypes = baseTypes;
		}

		public ItemType Parse(Data.Model.JSonProxy.Item item)
		{
			if (itemTypes == null)
				return ParseWithBaseTypes(item);

			return ParseWithItemTypes(item);
		}

		private ItemType ParseWithItemTypes(Data.Model.JSonProxy.Item item)
		{
			if (string.IsNullOrWhiteSpace(item.name))
				return itemTypes
					.Where(i => i.BaseType != "SkillGem Vaal")
					.FirstOrDefault(i => item.typeLine.Contains(i.Type)) ??
				       new ItemType {Category = ItemCategories.Unknown, Type = item.typeLine, BaseType = ItemType.UnknownBaseType};

			return itemTypes.SingleOrDefault(i => i.Type == item.typeLine) ?? new ItemType {Category = ItemCategories.Unknown, Type = item.typeLine, BaseType = ItemType.UnknownBaseType};
		}

		private ItemType ParseWithBaseTypes(Data.Model.JSonProxy.Item item)
		{
			var baseType = baseTypes
				.FirstOrDefault(b => b.TypeLines.Contains(item.typeLine)) ?? new BaseType {Name = ItemType.UnknownBaseType};

			return new ItemType {Category = ItemCategories.Unknown, Type = item.typeLine, BaseType = baseType.Name};
		}
	}
}
