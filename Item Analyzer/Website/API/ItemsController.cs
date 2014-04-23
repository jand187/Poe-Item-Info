using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Model;
using Website.Properties;

namespace Website.API
{
	public class ItemsController : ApiController
	{
		private readonly IItemParser itemParser;

		public ItemsController(IItemParser itemParser)
		{
			this.itemParser = itemParser;
		}

		public ItemsController()
		{
			var typeCategoryMap = LoadStuff();

			var propertyParser = new PropertyParser();
			var requirementParser = new RequirementParser();
			var modsParser = new ModsParser();
			var itemTypeParser = new ItemTypeParser(typeCategoryMap);
			this.itemParser = new ItemParser(propertyParser, requirementParser, modsParser, itemTypeParser);
		}

		private IEnumerable<ItemType> LoadStuff()
		{
			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "categories.json");
			var contents = File.ReadAllText(filename);

			return JsonConvert.DeserializeObject<IEnumerable<ItemType>>(contents);
		}

		public dynamic Get(ItemCategories category)
		{
			var stashFiles = Directory.GetFiles(Settings.Default.DataDirectory, "*.json");

			var list = stashFiles
				.Select(File.ReadAllText)
				.Select(JsonConvert.DeserializeObject<Stash>)
				.SelectMany(stash => itemParser.Parse(stash));


			return new
			{
				Success = true,
				Items = list.Where(i => i.Category == category.ToString()),
			};
		}

		public dynamic Get()
		{
			var stashFiles = Directory.GetFiles(Settings.Default.DataDirectory, "*.json");

			var list = stashFiles
				.Select(File.ReadAllText)
				.Select(JsonConvert.DeserializeObject<Stash>)
				.SelectMany(stash => itemParser.Parse(stash));


			return new
			{
				Success = true,
				Items = list,
			};
		}
	}
}