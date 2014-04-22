using System.Linq;

namespace PoeItemInfo.Model.Mods
{
	public class UnknownMod : IItemMod
	{
		public string Name
		{
			get { return "Unknown mod"; }
		}

		public string DisplayText { get; set; }
	}

	public class UnknownModFactory : IItemModFactory
	{
		public IItemMod Create(string modString)
		{
			return new UnknownMod
			{
				DisplayText = string.Format("({0})", modString)
			};
		}
	}
}