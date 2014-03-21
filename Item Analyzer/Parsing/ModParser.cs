using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Parsing
{
	public class ModParser
	{
		private readonly Dictionary<string, Func<string, string, IItemMod>> patterns;

		public ModParser()
		{
			patterns = new Dictionary<string, Func<string, string, IItemMod>>
				{
					{@"^(?<value>\d+)% of Physical Attack Damage Leeched as (?<name>.+)$", (name, value) => new PercentageLeechedMod {Name = name, Value = value}},
					{@"^(?<value>\d+)% (?<name>.+)$", (name, value) => new PercentageIncreaseMod {Name = name, Value = value}},
					{@"^\+(?<value>\d+) (?<name>.+)$", (name, value) => new AddedValueMod {Name = name, Value = value}},
					{@"^\+(?<value>\d+)% (?<name>.+)$", (name, value) => new AddedPercentageMod {Name = name, Value = value}},
					{@"^Adds (?<value>\d+-\d+) (?<name>.+)$", (name, value) => new AddedDamageRangeMod {Name = name, Value = value}},
					{@"^(?<value>\d+.\d+) (?<name>.+)$", (name, value) => new RegenerationMod {Name = name, Value = value}},
				};
		}

		public IEnumerable<IItemMod> Parse(string text)
		{
			var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			return lines.Select(ParseLine).ToList();
		}

		public IItemMod ParseLine(string line)
		{
			foreach (var pattern in patterns)
			{
				var match = Regex.Match(line, pattern.Key);
				if (match.Success)
					return pattern.Value.Invoke(match.Groups["name"].Value, match.Groups["value"].Value);
			}

			return new UnknownMod();
		}
	}

	public interface IItemMod
	{
		string Name { get; set; }
		string Value { get; set; }
	}

	public class PercentageIncreaseMod : IItemMod
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class AddedValueMod : IItemMod
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class AddedPercentageMod : IItemMod
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class RegenerationMod : IItemMod
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class PercentageLeechedMod : IItemMod
	{
		private string name;
		public string Name
		{
			get { return string.Format("of Physical Attack Damage Leeched as {0}", name); }
			set { name = value; }
		}

		public string Value { get; set; }
	}

	public class AddedDamageRangeMod : IItemMod
	{
		public string Name { get; set; }

		public string Value
		{
			get { return string.Format("{0}-{1}", MinValue, MaxValue); }
			set
			{
				MinValue = Convert.ToInt32(value.Split('-').First());
				MaxValue = Convert.ToInt32(value.Split('-').Last());
			}
		}

		public int MinValue { get; private set; }
		public int MaxValue { get; private set; }
	}


	public class UnknownMod : IItemMod
	{
		public string Name
		{
			get { return "Unknown"; }
			set { throw new InvalidOperationException(); }
		}

		public string Value
		{
			get { return "Unknown"; }
			set { throw new InvalidOperationException(); }
		}
	}
}