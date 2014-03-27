using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Parsing;
using Xunit;

namespace ParsingTest
{
	public class AffixTableParserTest
	{
		[Fact]
		public void Can_Parse_Single_Value_Affixes()
		{
			var affixTableBuilder = new StringBuilder();
			affixTableBuilder.AppendLine("Lacquered	1	Base Physical Damage Reduction Rating	3 to 10");
			affixTableBuilder.AppendLine("Studded	18	Base Physical Damage Reduction Rating	11 to 35");
			affixTableBuilder.AppendLine("Ribbed	30	Base Physical Damage Reduction Rating	36 to 60");
			affixTableBuilder.AppendLine("Fortified	44	Base Physical Damage Reduction Rating	61 to 138");
			affixTableBuilder.AppendLine("Plated	57	Base Physical Damage Reduction Rating	139 to 322");

			var result = new AffixTableParser().Parse(affixTableBuilder.ToString()).ToList();
			result.Count().Should().Be(5);
			result.Single(a => a.Name == "Ribbed").Stats.First().Value.MinValue.Should().Be(36);
			result.Single(a => a.Name == "Ribbed").Stats.First().Value.MaxValue.Should().Be(60);
		}

		[Fact]
		public void Can_Parse_Multi_Value_Affixes()
		{
			var affixTableBuilder = new StringBuilder();
			affixTableBuilder.AppendLine("Beetle's	1	Local Physical Damage Reduction Rating +%\r\nBase Stun Recovery +%	6 to 14\r\n6 to 7");
			affixTableBuilder.AppendLine("Crab's	17	Local Physical Damage Reduction Rating +%\r\nBase Stun Recovery +%	15 to 23\r\n8 to 9");
			affixTableBuilder.AppendLine("Armadillo's	29	Local Physical Damage Reduction Rating +%\r\nBase Stun Recovery +%	24 to 32\r\n10 to 11");
			affixTableBuilder.AppendLine("Rhino's	42	Local Physical Damage Reduction Rating +%\r\nBase Stun Recovery +%	33 to 41\r\n12 to 13");
			affixTableBuilder.AppendLine("Elephant's	60	Local Physical Damage Reduction Rating +%\r\nBase Stun Recovery +%	42 to 50\r\n14 to 15Base");

			var result = new AffixTableParser().Parse(affixTableBuilder.ToString()).ToList();
			result.Count().Should().Be(5);
			result.Single(a => a.Name == "Ribbed").Stats.First().Value.MinValue.Should().Be(36);
			result.Single(a => a.Name == "Ribbed").Stats.First().Value.MaxValue.Should().Be(60);
		}
	}
}
