using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PoeItemInfo.Data.Model.JSonProxy;

namespace PoeItemInfo.Model
{
	public class ItemParser
	{
		public Item Parse(Data.Model.JSonProxy.Item item)
		{
			return new Item
			{
				Original = item,
				Name = item.name,
				TypeLine = item.typeLine,
				Identified = item.identified,
				Corrupted = item.corrupted,
				Quality = GetQuality(item),
			};
		}

		private int GetQuality(Data.Model.JSonProxy.Item item)
		{
			const string propertyName = "Quality";
			if (!item.properties.Any())
				return 0;

			if (item.properties.All(p => p.name != propertyName))
				return 0;

			var valueSet = item.properties.First(p => p.name == propertyName).values.First().ToString();
			return Convert.ToInt32(Regex.Match(valueSet, @"\d+").Value);
		}
	}
}