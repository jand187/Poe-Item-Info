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

		[Queryable]
		[HttpPost]
		public IQueryable<Item> GetItems(Specification specification)
		{
			var filters = new List<Func<Item, bool>>();

			if (!string.IsNullOrWhiteSpace(specification.Type))
				filters.Add(i => i.BaseType == specification.Type);

			return LoadItems(filters.ToArray());
		}

		[Queryable]
		public IQueryable<Item> GetItems()
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

		private List<BaseType> LoadItemTypeMap()
		{
			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "typeMap.json");
			if (!File.Exists(filename))
				return new List<BaseType>();

			var contents = File.ReadAllText(filename);
			return JsonConvert.DeserializeObject<List<BaseType>>(contents);
		}

		public IQueryable<Item> Post(IEnumerable<SimpleSpecification> specifications)
		{
			if (specifications.Any())
			{
				var filter = new Func<Item, bool>(i => i.Category == specifications.First().Value);
				return LoadItems(filter);
			}

			return LoadItems();
		}
	}

	public class Specification
	{
		public string Type { get; set; }
		public int NumberOfItems { get; set; }
	}

	public class SimpleSpecification
	{
		public string Property { get; set; }
		public string Value { get; set; }
		public string Operation { get; set; }
	}
}
