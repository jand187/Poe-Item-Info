using System.Linq;
using PoeItemInfo.Data.Model.JSonProxy;

namespace TestCommon
{
	public static class Weapons
	{
		public static GenericBuilder<Item> Weapon(this GenericBuilder<Item> @this)
		{
			@this
				.With(item => item.verified = false)
				.With(item => item.w = 2)
				.With(item => item.h = 3)
				.With(item => item.icon = "icon")
				.With(item => item.league = "Standard")
				.Build();

			return @this;
		}

		public static GenericBuilder<Item> OneHandedAxe(this GenericBuilder<Item> @this)
		{
			@this
				.Weapon()
				.With(item => item.properties = new[]
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
				});
			return @this;
		}
	}
}