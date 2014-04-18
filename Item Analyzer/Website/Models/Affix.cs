using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Website.Models
{
	public abstract class Affix
	{
		private static readonly Dictionary<string, Antlr.Runtime.Misc.Func<Match, Affix>> parsers = new Dictionary<string, Antlr.Runtime.Misc.Func<Match, Affix>>
		{
			{
				@"\+(?<value>\d+) to (?<name>[\w\W]+$)", match => new PlusToAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			}
			,
			{
				@"(?<value>\d+)% (?<name>increased [\w\W]+$)", match => new PercentIncreaseAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},
			{
				@"\+(?<value>\d+)% (?<name>to [\w\W]+$)", match => new PlusPercentageToAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},
			{
				@"(?<value>\d+) (?<name>[\w]+ Regenerated per second$)", match => new RegeneratedPerSecondAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},
			{
				@"\+(?<value>\d+) (?<name>[\w]+ gained for each enemy hit by your Attacks$)", match => new GainedPerHitAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},
			{
				@"(?<value>\d+)% (?<name>of Physical Attack Damage Leeched as [\w]+$)", match => new PercentageOfDamageGainedAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},
			{
				@"\+(?<value>\d+) (?<name>[\w]+ gained on Kill$)", match => new GainedOnKillAffix
				{
					Name = match.Groups["name"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},
			{
				@"(?<name1>Reflects) (?<value>\d+) (?<name2>Physical Damage to Melee Attackers$)", match => new ReflectAffix
				{
					Name1 = match.Groups["name1"].Value,
					Name2 = match.Groups["name2"].Value,
					Value = Convert.ToInt32(match.Groups["value"].Value),
				}
			},

			{
				@"Adds (?<low>\d+)-(?<high>\d+) (?<name>[\w\W]+$)", match => new AddsRangeAffix
				{
					Name = match.Groups["name"].Value,
					Low = Convert.ToInt32(match.Groups["low"].Value),
					High = Convert.ToInt32(match.Groups["high"].Value),
				}
			},
		};

		public static Affix Parse(string affixString)
		{
			foreach (var parser in parsers)
			{
				var match = Regex.Match(affixString, parser.Key, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					return parser.Value.Invoke(match);
				}
			}

			return new UnknownAffix
			{
				Text = affixString
			};
		}

		public abstract string Name { get; set; }

		public virtual string Style
		{
			get { return "normal-affix"; }
		}

		public abstract int Value { get; set; }
	}
}