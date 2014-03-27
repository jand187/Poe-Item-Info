using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Parsing
{
	public class AffixTableParser
	{
		public IEnumerable<Affix> Parse(string affixTable)
		{
			var list = new List<Affix>();
			var strings = Regex.Split(affixTable, @"$", RegexOptions.Multiline);
			var lines = affixTable.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			foreach (var line in lines)
			{
				var sections = line.Split(new[] {"\t"}, StringSplitOptions.RemoveEmptyEntries);
				list.Add(new Affix
				{
					Name = sections[0],
					Level = Convert.ToInt32(sections[1]),
					Stats = ParseStat(sections[2], sections[3]),
				});
			}

			return list;
		}

		private IEnumerable<Stat> ParseStat(string nameSection, string valueSection)
		{
			var names = nameSection.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var values = valueSection.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			var list = new List<Stat>();
			for (int index = 0; index < names.Length; index++)
			{
				var name = names[index];
				list.Add(new Stat
				{
					Name = name,
					Value = ParseAffixValue(values[index])
				});
			}
			return list;
		}

		private AffixValue ParseAffixValue(string text)
		{
			var values = text.Split(new[] {" to "}, StringSplitOptions.RemoveEmptyEntries);
			return new AffixValue
			{
				MinValue = Convert.ToInt32(values[0]),
				MaxValue = Convert.ToInt32(values[1]),
			};
		}
	}

	public class Affix
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public IEnumerable<Stat> Stats { get; set; }
	}

	public class Stat
	{
		public string Name { get; set; }
		public AffixValue Value { get; set; }
	}

	public class AffixValue
	{
		public int MinValue { get; set; }
		public int MaxValue { get; set; }
	}
}
