using System.Collections.Generic;
using System.Linq;
using PoeItemInfo.Model.Mods;

namespace PoeItemInfo.Model
{
	public interface IModsParser
	{
		IEnumerable<IItemMod> Parse(IEnumerable<string> mods);
	}

	public class ModsParser : IModsParser
	{
		private readonly IEnumerable<IItemModFactory> itemModFactories;

		public ModsParser(IEnumerable<IItemModFactory> itemModFactories)
		{
			this.itemModFactories = itemModFactories;
		}

		public ModsParser()
		{
			itemModFactories = new IItemModFactory[]
			{
				new PercentageIncreasedModFactory(),
				new PlusToModFactory(),
				new UnknownModFactory(),
			};
		}

		public IEnumerable<IItemMod> Parse(IEnumerable<string> mods)
		{
			var modList = new List<IItemMod>();

			if (mods == null)
				return modList;

			foreach (var mod in mods)
			{
				foreach (var factory in itemModFactories)
				{
					var itemMod = factory.Create(mod);
					if (itemMod != null)
					{
						modList.Add(itemMod);
						break;
					}
				}
			}

			//modList.Add(new UnknownMod {DisplayText = mod});
			
			return modList;
		}
	}
}