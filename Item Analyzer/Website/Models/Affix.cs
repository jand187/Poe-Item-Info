using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr.Runtime.Misc;

namespace Website.Models
{
	public class Affix
	{
		private static readonly Dictionary<string, Func<Match, Affix>> parsers = new Dictionary<string, Func<Match, Affix>>
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
		};

		public static Affix Parse(string affixString)
		{
			foreach (var parser in parsers)
			{
				var match = Regex.Match(affixString, parser.Key);
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

	public class ItemsListViewModel
	{
		public IEnumerable<ItemViewModel> ItemViewModel { get; set; }
		public int CurrentPage { get; set; }
	}
}