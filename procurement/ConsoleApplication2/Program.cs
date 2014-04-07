using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApplication2
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var filename = @"..\..\..\..\ItemData\jand187\tab50.json";
			var contents = GetFileContents(filename);
			var stash = JsonConvert.DeserializeObject<Stash>(contents);
		}

		private static string GetFileContents(string filename)
		{
			string contents;
			var fileStream = File.Open(filename, FileMode.Open);
			using (var reader = new StreamReader(fileStream))
			{
				return reader.ReadToEnd();
			}
		}
	}

	public class Stash
	{
		[JsonProperty("numTabs")]
		public int NumberOfTabs { get; set; }

		[JsonProperty("items")]
		public IEnumerable<Item> Items { get; set; }

		public IEnumerable<Tab> Tabs { get; set; }
	}

	[DebuggerDisplay("{Name}")]
	public class Item
	{
		public string Name { get; set; }
		public string InventoryId { get; set; }
		public string TypeLine { get; set; }
		public IEnumerable<string> ExplicitMods { get; set; }
	}

	[DebuggerDisplay("{Name}")]
	public class Tab
	{
		[JsonProperty("n")]
		public string Name { get; set; }
		
	}
}
