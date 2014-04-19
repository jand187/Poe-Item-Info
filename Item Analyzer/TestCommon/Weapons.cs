using System;
using System.Linq;
using PoeItemInfo.Data.Model.JSonProxy;

namespace TestCommon
{
	public static class PoeItem
	{
		public static GenericBuilder<Item> Item(this GenericBuilder<Item> @this)
		{
			@this
				.With(item => item.verified = false)
				.With(item => item.w = 0)
				.With(item => item.h = 0)
				.With(item => item.icon = "<icon>")
				.With(item => item.support = false)
				.With(item => item.league = "<league>")
				.With(item => item.sockets = new Socket[0])
				.With(item => item.name = "<name>")
				.With(item => item.typeLine = "<typeLine>")
				.With(item => item.identified = true)
				.With(item => item.corrupted = false)
				.With(item => item.frameType = 2)
				.With(item => item.x = 0)
				.With(item => item.y = 0)
				.With(item => item.inventoryId = "<inventoryId>")
				.With(item => item.socketedItems = new Item[0])
				.With(item => item.properties = item.properties ?? new Property[0])
				;

			return @this;
		}

		public static GenericBuilder<Item> Quality(this GenericBuilder<Item> @this, int quality)
		{
			@this
				.Item()
				.With(item => item.properties = item.properties.Concat(new[]
				{
					new Property
					{
						displayMode = 0,
						name = "Quality",
						values = new Object[] {string.Format("+{0}%", quality), 1}
					},
				}));

			return @this;
		}
	}


	public static class Armour
	{
		public static GenericBuilder<Item> Chestarmour(this GenericBuilder<Item> @this)
		{
			@this
				.Item()
				.With(item => item.properties = item.properties.Concat(new[]
				{
					new Property
					{
						displayMode = 0,
						name = "Armour",
						values = new Object[] {"347", 1}
					},
					new Property
					{
						displayMode = 0,
						name = "Physical Damage",
						values = new object[] {"44-82", 1}
					},
				}));

			return @this;
		}

		public static GenericBuilder<Item> Helmet(this GenericBuilder<Item> @this)
		{
			return @this;
		}
	}


	public static class Weapons
	{
		public static GenericBuilder<Item> Weapon(this GenericBuilder<Item> @this)
		{
			return @this;
		}

		public static GenericBuilder<Item> OneHandedAxe(this GenericBuilder<Item> @this)
		{
			@this
				.Item()
				.Weapon()
				.With(item => item.properties = item.properties.Concat(new[]
				{
					new Property
					{
						displayMode = 0,
						name = "One Handed Axe",
						values = new object[0]
					},
					new Property
					{
						displayMode = 0,
						name = "Physical Damage",
						values = new object[] {"44-82", 1}
					},
				}));
			return @this;
		}
	}
}