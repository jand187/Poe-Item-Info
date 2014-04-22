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
			var modString = "39% increased Armour and Evasion";
			var mods = new []
			{
				modString,
			};

			var mod = new ModsParser().Parse(mods).Single();
			mod.Should().BeOfType<PercentageIncreasedMod>();
			mod.DisplayText.Should().Be(modString);
		}

		[Fact]
		public void ShouldParsePlusToMod()
		{
			var modString = "+23 to Strength";
			var mods = new[]
			{
				modString,
			};

			var mod = new ModsParser().Parse(mods).Single();
			mod.Should().BeOfType<PlusToMod>();
			mod.DisplayText.Should().Be(modString);
		}

	}
}