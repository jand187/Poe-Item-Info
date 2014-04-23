using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PoeItemInfo.Model;

namespace PoeItemInfo.Website.Api
{
	public class BaseTypesController : ApiController
	{
		public IEnumerable<dynamic> Get()
		{
			var specification = new Specification
			{
				Property = "Category",
				Operation = "eq",
				Value = ItemCategories.Unknown.ToString()
			};

			var specifications = new Specification[0];
			var items = new ItemsController().Post(specifications);

			return items.Select(item => new {item.TypeLine, item.ItemType}).Distinct();
		}

		// return items.Where(item => item.ItemType.Category == ItemCategories.Unknown).Select(item => item.TypeLine).Distinct();
	}
}
