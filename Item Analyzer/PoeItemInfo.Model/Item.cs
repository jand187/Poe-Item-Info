using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Model
{
	public class Item
	{
		public Data.Model.JSonProxy.Item Original { get; set; }

		public string Name { get; set; }
		public string Icon { get; set; }
		public string TypeLine { get; set; }
		public bool Identified { get; set; }
		public bool Corrupted { get; set; }
		public int Quality { get; set; }
		public IEnumerable<ItemProperty> Properties { get; set; }
		public IEnumerable<ItemRequirement> Requirements { get; set; }
		public IEnumerable<IItemMod> ExplicitMods { get; set; }
		public IEnumerable<IItemMod> ImplicitMods { get; set; }
		public ItemLocation Location { get; set; }
		public IEnumerable<Item> SocketedItems { get; set; }
		public int FrameType { get; set; }
		public ItemType ItemType { get; set; }
		
		public string BaseType { get { return ItemType.BaseType; } }
		public string Category { get { return ItemType.Category.ToString(); } }
	}

	public class ItemProperty
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class ItemRequirement
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public interface IItemMod
	{
		string Name { get;  }
		string DisplayText { get;  }
	}

	public interface IItemModFactory
	{
		IItemMod Create(string modString);
	}

	public class ItemLocation
	{
		public string InventoryId { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}

	public class ItemType
	{
		public string Type { get; set; }
		public ItemCategories Category { get; set; }
		public string BaseType { get; set; }
	}

	public class ItemTypeX
	{
		public string Type { get; set; }
		public string Category { get; set; }
		public string BaseType { get; set; }
	}

	public enum ItemCategories
	{
		Unknown,
		Weapon,
		Armour,
		Jewelery,
		SkillGem,
		Currency,
	}
}