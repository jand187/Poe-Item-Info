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
		private readonly IEnumerable<ItemType> itemTypes;

		public ItemTypeParser(IEnumerable<ItemType> itemTypes)
		{
			this.itemTypes = itemTypes;
		}

		public ItemType Parse(Data.Model.JSonProxy.Item item)
		{
			if (string.IsNullOrWhiteSpace(item.name))
				return itemTypes
					.Where(i => i.BaseType != "SkillGem Vaal")
					.FirstOrDefault(i => item.typeLine.Contains(i.Type)) ??
				       new ItemType {Category = ItemCategories.Unknown, Type = item.typeLine, BaseType = "Unknown"};

			return itemTypes.SingleOrDefault(i => i.Type == item.typeLine) ?? new ItemType {Category = ItemCategories.Unknown, Type = item.typeLine, BaseType = "Unknown"};
		}
	}
}