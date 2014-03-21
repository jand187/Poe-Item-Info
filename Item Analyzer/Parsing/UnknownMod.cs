using System;
using System.Linq;

namespace Parsing
{
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