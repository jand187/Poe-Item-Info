using System.Collections.Generic;
using System.Linq;

namespace PoeItemInfo.Model
{
	public interface IModsParser
	{
		IEnumerable<ItemMod> Parse(IEnumerable<string> mods);
	}

	public class ModsParser : IModsParser
	{
		public IEnumerable<ItemMod> Parse(IEnumerable<string> mods)
		{
			return null;
		}
	}
}