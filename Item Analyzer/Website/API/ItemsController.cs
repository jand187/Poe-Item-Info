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

		public ItemsController() : this(new ItemParser(new PropertyParser(), new RequirementParser(), new ModsParser(), new ItemTypeParser()))
		{
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
				Items = list.Where(i=>i.Category == ItemCategory.Unknown.ToString()),
			};
		}
	}
}