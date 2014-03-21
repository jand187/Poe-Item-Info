using System;
using System.Linq;

namespace Parsing
{
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
}