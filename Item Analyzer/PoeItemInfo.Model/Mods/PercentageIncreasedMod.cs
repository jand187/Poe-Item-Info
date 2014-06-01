﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PoeItemInfo.Model.Mods
{
	public class PercentageIncreasedMod : IItemMod
	{
		public string Name { get; set; }

		public string DisplayText
		{
			get { return string.Format("{0}% {1}", Value, Name); }
		}

		public int Value { get; set; }
	}

	public class PercentageIncreasedModFactory : IItemModFactory
	{
		public IItemMod Create(string modString)
		{
			var match = Regex.Match(modString, @"^(?<value>\d+)% (?<name>[\w ]+)$");
			if (!match.Success)
				return null;

			return new PercentageIncreasedMod
			{
				Name = match.Groups["name"].Value,
				Value = Convert.ToInt32(match.Groups["value"].Value)
			};
		}
	}
} 
