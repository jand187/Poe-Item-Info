using System.Collections.Generic;
using System.Linq;
using PoeItemInfo.Data.Model.JSonProxy;

namespace PoeItemInfo.Model
{
	public interface IPropertyParser
	{
		IEnumerable<ItemProperty> Parse(IEnumerable<Property> properties);
	}

	public class PropertyParser : IPropertyParser
	{
		public IEnumerable<ItemProperty> Parse(IEnumerable<Property> properties)
		{
			return null;
		}
	}
}