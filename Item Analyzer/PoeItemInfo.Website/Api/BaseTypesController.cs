using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Model;
using PoeItemInfo.Website.Properties;

namespace PoeItemInfo.Website.Api
{
	public class BaseTypesController : ApiController
	{
		public IEnumerable<dynamic> Get()
		{
			var specification = new SimpleSpecification
			{
				Property = "Category",
				Operation = "eq",
				Value = ItemCategories.Unknown.ToString()
			};

			var specifications = new SimpleSpecification[0];
			var items = new ItemsController().Post(specifications);

			return items.Select(item => new {item.TypeLine, item.ItemType}).Distinct();
		}

		public IEnumerable<string> GetTypeLines(string keyword)
		{
			var typeLines = GetTypeLines();
			if (string.IsNullOrWhiteSpace(keyword))
				return typeLines;

			return typeLines
				.Where(t => t.Contains(keyword));
		}

		public IEnumerable<string> GetTypeLines()
		{
			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "typeMap.json");
			var baseTypes = LoadFile<List<BaseType>>(filename);
			var knownTypeLines = baseTypes.SelectMany(b => b.TypeLines);

			return new ItemsController()
				.GetItems()
				.Where(i => i.ItemType.BaseType == ItemType.UnknownBaseType)
				.Where(i => TypeUnknown(i.TypeLine, knownTypeLines))
				.Select(i => i.TypeLine)
				.Distinct();
		}

		private bool TypeUnknown(string typeLine, IEnumerable<string> knownTypeLines)
		{
			if (knownTypeLines.Contains(typeLine))
				return false;

			foreach (var knownTypeLine in knownTypeLines.Where(k => !k.StartsWith("Vaal")))
			{
				if (typeLine.Contains(knownTypeLine))
					return false;

				if (typeLine.Contains(string.Format("{0} of", knownTypeLine)))
					return false;

				if (typeLine.Contains(string.Format("Superior {0}", knownTypeLine)))
					return false;
			}

			return true;
		}

		public void SaveTypeMap(IEnumerable<TypeMapItem> items)
		{
			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "typeMap.json");

			var baseTypes = LoadFile<List<BaseType>>(filename);

			foreach (var item in items)
			{
				var baseType = baseTypes.SingleOrDefault(c => c.Name == item.Type);
				if (baseType == null)
				{
					baseType = new BaseType {Name = item.Type, TypeLines = new List<string>()};
					baseTypes.Add(baseType);
				}
				var typeLine = Regex.Replace(item.TypeLine, @"^Superior ", "");
				if (baseType.TypeLines.Any(t => t == typeLine)) continue;
				baseType.TypeLines.Add(typeLine);
			}

			var baseTypesJson = JsonConvert.SerializeObject(baseTypes);
			using (var fileStream = File.OpenWrite(filename))
			{
				using (var writer = new StreamWriter(fileStream))
				{
					writer.Write(baseTypesJson);
					writer.Flush();
				}
			}
		}

		private TType LoadFile<TType>(string filename) where TType : class, new()
		{
			if (!File.Exists(filename))
				return new TType();

			string contents;
			using (var fileStream = File.OpenRead(filename))
			{
				using (var reader = new StreamReader(fileStream))
				{
					contents = reader.ReadToEnd();
				}
			}

			if (string.IsNullOrEmpty(contents))
				return new TType();

			return JsonConvert.DeserializeObject<TType>(contents);
		}

		public IEnumerable<string> GetTypes()
		{
			return LoadList();
		}

		private IEnumerable<string> LoadList()
		{
			var filename = Path.Combine(Settings.Default.DataBaseDirectory, "types.json");
			if (!File.Exists(filename))
				return new string[0];

			var contents = File.ReadAllText(filename);
			return JsonConvert.DeserializeObject<IEnumerable<string>>(contents);
		}


		// return items.Where(item => item.ItemType.Category == ItemCategories.Unknown).Select(item => item.TypeLine).Distinct();
	}

	public class TypeMapItem
	{
		public string TypeLine { get; set; }
		public string Type { get; set; }
	}
}
