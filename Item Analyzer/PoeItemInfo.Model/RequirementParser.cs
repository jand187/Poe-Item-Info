using System.Collections.Generic;
using System.Linq;
using PoeItemInfo.Data.Model.JSonProxy;

namespace PoeItemInfo.Model
{
	public interface IRequirementParser
	{
		IEnumerable<ItemRequirement> Parse(IEnumerable<Requirement> properties);
	}

	public class RequirementParser : IRequirementParser
	{
		public IEnumerable<ItemRequirement> Parse(IEnumerable<Requirement> properties)
		{
			return null;
		}
	}
}