using System;
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

		public dynamic GetTab(int stashTab)
		{
			var pwFile = File.ReadAllText(Settings.Default.PasswordFile);
			var creds = JsonConvert.DeserializeObject<Credentials>(pwFile);

			var securePassword = new SecureString();
			creds.Password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			transport.Authenticate(creds.Username, securePassword, false);

			var json = transport.GetStashJson(stashTab, "Standard");
			var stash = JsonConvert.DeserializeObject<Stash>(json);

			var filename = Path.Combine(Settings.Default.OfficialFiles, string.Format("Stash{0}.json", stashTab));
			CreateFile(json, filename);

			return new
			{
				Success = true,
				Index = stashTab,
				Name = stash.tabs.Single(t => t.i == stashTab).n,
				Loaded = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"),
				NumberOfItems = stash.items.Count(),
				NumberOfTabs = stash.numTabs,
				Tabs = stash.tabs.Select(s => new {Name = s.n, Id = s.i, LastUpdated = GetLastModified(s.i)})
			};
		}

		private string GetLastModified(int index)
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
