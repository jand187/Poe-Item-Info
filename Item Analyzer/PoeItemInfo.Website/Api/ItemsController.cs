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
		private IItemParser itemParser;

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
			var filter = new Func<Item, bool>(i => i.Category == specification.First().Value);
			return LoadItems(filter);
		}

		public IEnumerable<Item> Get()
		{
			return new[]
			{
				new Item
				{
					Name = "Horror Grip",
					ItemType = new ItemType
					{
						BaseType = "Gloves",
						Category = ItemCategories.Armour,
						Type = "Samite Gloves"
					},
					Icon = @"http:\/\/webcdn.pathofexile.com\/image\/Art\/2DItems\/Armours\/Helmets\/HelmetStrInt4.png?scale=1&w=2&h=2&v=fa25d2b751889f688486d20fb79dacb93",
				},
				new Item
				{
					Name = "Wideswing",
					ItemType = new ItemType
					{
						BaseType = "Two Handed Axe",
						Category = ItemCategories.Weapon,
						Type = "Poleaxe"
					},
					Icon = @"http:\/\/webcdn.pathofexile.com\/image\/Art\/2DItems\/Weapons\/TwoHandWeapons\/TwoHandAxes\/Wideswing.png?scale=1&w=2&h=4&v=abf4d162c7297ee1d6871ec1f24afa583",
				},
				new Item
				{
					Name = "Bramble Crest",
					ItemType = new ItemType
					{
						BaseType = "Helmet",
						Category = ItemCategories.Armour,
						Type = "Ursine Pelt"
					},
					Icon = @"http:\/\/webcdn.pathofexile.com\/image\/Art\/2DItems\/Armours\/Helmets\/HelmetDex7.png?scale=1&w=2&h=2&v=8d01ba9ab32a8d671bcc66c25c0e90ec3",
				}
			};
		}



		private IEnumerable<Item> LoadItems(params Func<Item, bool>[] predicates)
		{
			var stashFiles = Directory.GetFiles(Settings.Default.OfficialFiles, "*.json");

			var list = stashFiles
				.Select(File.ReadAllText)
				.Select(JsonConvert.DeserializeObject<Stash>)
				.SelectMany(stash => itemParser.Parse(stash));

			foreach (var predicate in predicates)
			{
				list = list.Where(i=> predicate.Invoke(i));
			}

			return list;
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