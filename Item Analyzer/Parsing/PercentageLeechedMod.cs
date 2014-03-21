using System.Linq;

namespace Parsing
{
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
}