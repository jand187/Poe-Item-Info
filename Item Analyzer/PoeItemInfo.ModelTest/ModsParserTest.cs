using System.Linq;
using FluentAssertions;
using PoeItemInfo.Model;
using PoeItemInfo.Model.Mods;
using Xunit;

namespace PoeItemInfo.ModelTest
{
	public class ModsParserTest
	{
		[Fact]
		public void ShouldParseunknownMod()
		{
			var mods = new[]
			{
				"*** 39% increased Armour and Evasion",
			};

			var itemMods = new ModsParser().Parse(mods);
			itemMods.First().Should().BeOfType<UnknownMod>();
		}


		[Fact]
		public void ShouldParsePercentageIncreasedMod()
		{
			var mods = new []
			{
				"39% increased Armour and Evasion",
			};

			var itemMods = new ModsParser().Parse(mods);
			itemMods.First().Should().BeOfType<PercentageIncreasedMod>();
		}
	}
}