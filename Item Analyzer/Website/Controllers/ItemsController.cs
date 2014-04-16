using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Website.JSonProxies;
using Website.Models;

namespace Website.Controllers
{
	public class ItemsController : Controller
	{
		public ActionResult Index(int page = 50)
		{
			var tabDatafilePath = string.Format(@"C:\Users\janand\Documents\GitHub\Poe-Item-Info\ItemData\jand187\tab{0}.json", page);
			var contents = System.IO.File.ReadAllText(tabDatafilePath);
			var stashTab = JsonConvert.DeserializeObject<StashTab>(contents);

			var model = new ItemsListViewModel
			{
				CurrentPage = page,
				ItemViewModel = stashTab.Items.Where(e => !string.IsNullOrWhiteSpace(e.Name)).Select(item => new ItemViewModel
				{
					Name = item.Name,
					Affixes = item.ExplicitMods.Select(Affix.Parse)
				})
			};

			return View(model);
		}
	}
}