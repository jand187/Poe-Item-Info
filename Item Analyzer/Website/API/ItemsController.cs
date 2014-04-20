using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Model;
using Website.Properties;
using Item = PoeItemInfo.Model.Item;

namespace Website.API
{
	public class ItemsController : ApiController
	{
		private IItemParser itemParser;

		public ItemsController(IItemParser itemParser)
		{
			this.itemParser = itemParser;
		}

		public ItemsController() : this(new ItemParser(new PropertyParser(), new RequirementParser(), new ModsParser()))
		{
		}

		public dynamic Get()
		{
			var stashFiles = Directory.GetFiles(Settings.Default.DataDirectory, "*.json");
			var contents = File.ReadAllText(stashFiles.First());

			var stash = JsonConvert.DeserializeObject<Stash>(contents);

			var items = stash.items.Select(i => itemParser.Parse(i));

			return new
			{
				Success = true,
				Items = items,
			};
		}
	}
}