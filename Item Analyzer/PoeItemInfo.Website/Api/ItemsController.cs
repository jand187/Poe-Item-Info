using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Model;
using PoeItemInfo.Website.Properties;
using Item = PoeItemInfo.Model.Item;

namespace PoeItemInfo.Website.Api
{
	public class ItemsController : ApiController
	{
		private readonly IItemParser itemParser;

		public ItemsController()
		{
			var typeCategoryMap = LoadItemTypeMap();

			var propertyParser = new PropertyParser();
			var requirementParser = new RequirementParser();
			var modsParser = new ModsParser();
			var itemTypeParser = new ItemTypeParser(typeCategoryMap);
			this.itemParser = new ItemParser(propertyParser, requirementParser, modsParser, itemTypeParser);
		}


		public IEnumerable<Item> Post(IEnumerable<Specification> specification)
		{
			if (specification.Any())
			{
				var filter = new Func<Item, bool>(i => i.Category == specification.First().Value);
				return LoadItems(filter);
			}

			return LoadItems();
		}

		[Queryable]
		public IQueryable<Item> Get()
		{
			return LoadItems();
		}


		private IQueryable<Item> LoadItems(params Func<Item, bool>[] predicates)
		{
			var stashFiles = Directory.GetFiles(Settings.Default.OfficialFiles, "*.json");

			var list = stashFiles.AsQueryable()
				.Select(File.ReadAllText)
				.Select(JsonConvert.DeserializeObject<Stash>)
				.SelectMany(stash => itemParser.Parse(stash));

			return predicates.Aggregate(list, (current, predicate) => current.Where(predicate.Invoke)).AsQueryable();
		}

		private IEnumerable<ItemType> LoadItemTypeMap()
		{
			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "categories.json");
			if (!File.Exists(filename))
				return new ItemType[0];

			var contents = File.ReadAllText(filename);
			return JsonConvert.DeserializeObject<IEnumerable<ItemType>>(contents);
		}
	}

	public class Specification
	{
		public string Property { get; set; }
		public string Value { get; set; }
		public string Operation { get; set; }
	}
}