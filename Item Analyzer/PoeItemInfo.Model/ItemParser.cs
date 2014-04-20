using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PoeItemInfo.Model
{
	public interface IItemParser
	{
		Item Parse(Data.Model.JSonProxy.Item item);
	}

	public class ItemParser : IItemParser
	{
		private readonly IPropertyParser propertyParser;
		private readonly IRequirementParser requirementParser;
		private readonly IModsParser modsParser;

		public ItemParser(IPropertyParser propertyParser, IRequirementParser requirementParser, IModsParser modsParser)
		{
			this.propertyParser = propertyParser;
			this.requirementParser = requirementParser;
			this.modsParser = modsParser;
		}

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
				Properties = propertyParser.Parse(item.properties),
				Requirements = requirementParser.Parse(item.requirements),
				ExplicitMods = modsParser.Parse((item.explicitMods)),
				ImplicitMods = modsParser.Parse((item.implicitMods)),
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