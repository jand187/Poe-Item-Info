using System.Linq;
using System.Text.RegularExpressions;

namespace PoeItemInfo.Model.Mods
{
	public class PercentageIncreasedMod : IItemMod
	{
		public string Name { get; set; }
	}

	public class PercentageIncreasedModFactory : IItemModFactory
	{
		public IItemMod Create(string modString)
		{
			var match = Regex.Match(modString, @"^39% (?<name>increased Armour and Evasion)$");
			if (!match.Success)
				return new UnknownMod();

			return new PercentageIncreasedMod
			{
				Name = match.Groups["name"].Value
			};
		}
	}
} ;