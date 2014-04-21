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
				new UnknownModFactory(),
			};
		}

		public IEnumerable<IItemMod> Parse(IEnumerable<string> mods)
		{
			if (mods == null)
				yield break;

			foreach (var mod in mods)
			{
				IItemMod itemMod = new UnknownMod();
				foreach (var factory in itemModFactories)
				{
					itemMod = factory.Create(mod);
					if (itemMod.GetType() != typeof (UnknownMod))
					{
						yield return itemMod;
					}
				}

				yield return itemMod;
			}
		}
	}
}