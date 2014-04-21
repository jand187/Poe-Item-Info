using System.Linq;

namespace PoeItemInfo.Model.Mods
{
	public class UnknownMod : IItemMod
	{
		public string Name { get; set; }
	}

	public class UnknownModFactory : IItemModFactory
	{
		public IItemMod Create(string modString)
		{
			return new UnknownMod
			{
				Name = modString
			};
		}
	}
}