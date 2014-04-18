using System;
using System.Linq;

namespace PoeItemInfo.Model
{
	public class ItemParser
	{
		public Item Parse(Data.Model.JSonProxy.Item item)
		{
			return new Item
			{
				Type = item.properties.First().name
			};
		}
	}
}