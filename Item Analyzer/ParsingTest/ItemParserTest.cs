﻿using System.Linq;
using System.Text;
using FluentAssertions;
using Parsing;
using Xunit;

namespace ParsingTest
{
	public class ItemParserTest
	{
		[Fact]
		public void Can_Parse_PercentageIncreaseMod()
		{
			const string text = "21% increased Elemental Damage with Weapons";

			var result = new ItemParser().ParseLine(text) as PercentageIncreaseMod;

			result.Should().NotBeNull();
			result.Name.Should().Be("increased Elemental Damage with Weapons");
			result.Value.Should().Be("21");
		}

		[Fact]
		public void Can_Parse_AddedValueMod()
		{
			const string text = "+43 to Armour";

			var result = new ItemParser().ParseLine(text) as AddedValueMod;

			result.Should().NotBeNull();
			result.Name.Should().Be("to Armour");
			result.Value.Should().Be("43");
		}

		[Fact]
		public void Can_Parse_AddedPercentageMod()
		{
			const string text = "+11% to Lightning Resistance";

			var result = new ItemParser().ParseLine(text) as AddedPercentageMod;

			result.Should().NotBeNull();
			result.Name.Should().Be("to Lightning Resistance");
			result.Value.Should().Be("11");
		}

		[Fact]
		public void Can_Parse_AddedDamageRangeMod()
		{
			const string text = "Adds 9-15 Fire Damage";

			var result = new ItemParser().ParseLine(text) as AddedDamageRangeMod;

			result.Should().NotBeNull();
			result.Name.Should().Be("Fire Damage");
			result.Value.Should().Be("9-15");
			result.MinValue.Should().Be(9);
			result.MaxValue.Should().Be(15);
		}

		[Fact]
		public void Can_Parse_RegenerationMod()
		{
			const string text = "3.8 Life Regenerated per second";

			var result = new ItemParser().ParseLine(text) as RegenerationMod;

			result.Should().NotBeNull();
			result.Name.Should().Be("Life Regenerated per second");
			result.Value.Should().Be("3.8");
		}

		[Fact]
		public void Can_Parse_PercentageLeechedMod()
		{
			const string text = "1% of Physical Attack Damage Leeched as Life";

			var result = new ItemParser().ParseLine(text) as PercentageLeechedMod;

			result.Should().NotBeNull();
			result.Name.Should().Be("of Physical Attack Damage Leeched as Life");
			result.Value.Should().Be("1");
		}

		[Fact]
		public void Parse_Differentiates_Between_Mod_Types()
		{
			var builder = new StringBuilder();
			builder.AppendLine("+43 to Armour");
			builder.AppendLine("+11% to Lightning Resistance");
			builder.AppendLine("21% increased Elemental Damage with Weapons");
			builder.AppendLine("+11% to Lightning Resistance");
			builder.AppendLine("+43 to Armour");
			builder.AppendLine("21% increased Elemental Damage with Weapons");

			var result = new ItemParser().Parse(builder.ToString()).ToList();

			result[0].GetType().Should().Be(typeof (AddedValueMod));
			result[1].GetType().Should().Be(typeof (AddedPercentageMod));
			result[2].GetType().Should().Be(typeof (PercentageIncreaseMod));
			result[3].GetType().Should().Be(typeof (AddedPercentageMod));
			result[4].GetType().Should().Be(typeof (AddedValueMod));
			result[5].GetType().Should().Be(typeof (PercentageIncreaseMod));
		}
	}
}
