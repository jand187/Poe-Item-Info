using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PoeItemInfo.Model;

namespace Website.Controllers
{
	public class CategoriesController : Controller
	{
		public ActionResult Index()
		{
			var x = new API.ItemsController();
			var o = x.Get();

			var items = o.Items as IEnumerable<Item>;

//			var basetypes = items.Select(i => i.TypeLine).Distinct();

			var model = new CategoriesControllerModel
			{
				Items = items,
			};
			return View(model);
		}
	}

	public class CategoriesControllerModel
	{
		public IEnumerable<Item> Items { get; set; }
	}
}