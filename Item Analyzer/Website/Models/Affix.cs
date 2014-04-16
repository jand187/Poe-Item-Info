using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Website.Models
{
	public class Affix
	{
		private static readonly Dictionary<string, Antlr.Runtime.Misc.Func<Match, Affix>> parsers = new Dictionary<string, Antlr.Runtime.Misc.Func<Match, Affix>>
		{
			{
				@"\+(?<value>\d+) to (?<name>[\w\W]+$)", match => new PlusToAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			}
			,
			{
				@"(?<value>\d+)% (?<name>increased [\w\W]+$)", match => new PercentIncreaseAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			},
			{
				@"\+(?<value>\d+)% (?<name>to [\w\W]+$)", match => new PlusPercentageToAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			},
			{
				@"(?<value>\d+) (?<name>[\w]+ Regenerated per second$)", match => new RegeneratedPerSecondAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			},
			{
				@"\+(?<value>\d+) (?<name>[\w]+ gained for each enemy hit by your Attacks$)", match => new GainedPerHitAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			},
			{
				@"(?<value>\d+)% (?<name>of Physical Attack Damage Leeched as [\w]+$)", match => new PercentageOfDamageGainedAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			},
			{
				@"\+(?<value>\d+) (?<name>[\w]+ gained on Kill$)", match => new GainedOnKillAffix
				{
					Name = match.Groups["name"].Value,
					Value = match.Groups["value"].Value,
				}
			},
						{
				@"(?<name1>Reflects) (?<value>\d+) (?<name2>Physical Damage to Melee Attackers$)", match => new ReflectAffix
				{
					Name1 = match.Groups["name1"].Value,
					Name2 = match.Groups["name2"].Value,
					Value = match.Groups["value"].Value,
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
				var match = Regex.Match(affixString, parser.Key,RegexOptions.IgnoreCase);
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

		public virtual string Style
		{
			get { return "normal-affix"; }
		}
	}

	public class ReflectAffix : Affix
	{
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("{2} {0} {1} ({3})", Value, Name2, Name1, this.GetType().Name);
		}
	}

	public class GainedOnKillAffix : Affix
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("+{0} {1} ({2})", Value, Name, this.GetType().Name);
		}
	}

	public class PercentageOfDamageGainedAffix : Affix
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("{0}% {1} ({2})", Value, Name, this.GetType().Name);
		}
	}

	public class GainedPerHitAffix : Affix
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("+{0} {1} ({2})", Value, Name, this.GetType().Name);
		}
	}

	public class AddsRangeAffix : Affix
	{
		public string Name { get; set; }
		public int Low { get; set; }
		public int High { get; set; }

		public override string ToString()
		{
			return string.Format("Adds {0}-{1} {2} ({3})", Low, High, Name, this.GetType().Name);
		}
	}

	public class ItemsListViewModel
	{
		public IEnumerable<ItemViewModel> ItemViewModel { get; set; }
		public int CurrentPage { get; set; }
	}
}