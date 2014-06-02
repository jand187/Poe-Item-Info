using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Transport;
using PoeItemInfo.Website.Models;
using PoeItemInfo.Website.Properties;

namespace PoeItemInfo.Website.Api
{
	public class StashController : ApiController
	{
		private readonly IHttpTransport transport;

		public StashController(IHttpTransport transport)
		{
			this.transport = transport;
		}

		public StashController()
			: this(new HttpTransport())
		{
		}

		public dynamic GetCharacters()
		{
			Authenticate();

			var charactersJson = transport.GetCharactersJson();
			var characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(charactersJson).ToList();

			return new
			{
				Success = true,
				Index = -1,
				Name = "not set",
				Loaded = "not set",
				NumberOfItems = -1,
				NumberOfTabs = characters.Count(),
				Tabs = characters.Select(c => new {Name = c.name, Id = c.name, LastUpdated = GetLastModified(c.name), Type = "inventory"})
			};

			return characters;
		}

		public dynamic GetCharacterTab(string character)
		{
			Authenticate();

			var charactersJson = transport.GetCharacterTabJson(character);
			var characters = JsonConvert.DeserializeObject<Stash>(charactersJson);

			return characters;
		}

		public dynamic GetTab(int stashTab)
		{
			Authenticate();

			var stashJson = transport.GetStashJson(stashTab, "Standard");
			var stash = JsonConvert.DeserializeObject<Stash>(stashJson);

			var filename = Path.Combine(Settings.Default.OfficialFiles, string.Format("Stash{0}.json", stashTab));
			CreateFile(stashJson, filename);

			return new
			{
				Success = true,
				Index = stashTab,
				Name = stash.tabs.Single(t => t.i == stashTab).n,
				Loaded = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"),
				NumberOfItems = stash.items.Count(),
				NumberOfTabs = stash.numTabs,
				Tabs = stash.tabs.Select(s => new {Name = s.n, Id = s.i, LastUpdated = GetLastModified(s.i), Type = "stash"})
			};
		}

		private void Authenticate()
		{
			var pwFile = File.ReadAllText(Settings.Default.PasswordFile);
			var creds = JsonConvert.DeserializeObject<Credentials>(pwFile);

			var securePassword = new SecureString();
			creds.Password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			transport.Authenticate(creds.Username, securePassword, false);
		}

		private string GetLastModified(int index)
		{
			return GetLastModified(index.ToString());
		}

		private string GetLastModified(string index)
		{
			var filename = Path.Combine(Settings.Default.OfficialFiles, string.Format("Stash{0}.json", index));
			if (!File.Exists(filename))
				return "Never";

			return File.GetLastWriteTime(filename).ToString();
		}

		private static void CreateFile(string json, string filename)
		{
			using (var writer = File.CreateText(filename))
			{
				writer.Write(json);
			}
		}
	}
}
