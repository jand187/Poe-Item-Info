using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Transport;
using Website.Properties;

namespace Website.API
{
	public class StashController : ApiController
	{
		private readonly IHttpTransport transport;

		public StashController(IHttpTransport transport)
		{
			this.transport = transport;
		}

		public StashController() : this(new HttpTransport())
		{
		}

		public dynamic Get(string email, string password, int stashTab)
		{
			var securePassword = new SecureString();
			password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			transport.Authenticate(email, securePassword, false);


			var json = transport.GetStashJson(stashTab, "Standard");
			var tab = JsonConvert.DeserializeObject<Stash>(json);

			var filename = Path.Combine(Settings.Default.DataDirectory, string.Format("Stash{0}.json", stashTab));
			CreateFile(stashTab, json, filename);

			return new
			{
				Success = true,
				Index = stashTab,
				Name = tab.tabs.Single(t => t.i == stashTab).n,
				Loaded = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss"),
				NumberOfItems = tab.items.Count(),
				NumberOfTabs = tab.numTabs,
			};
		}

		private static void CreateFile(int stashTab, string json, string filename)
		{
			using (var writer = File.CreateText(filename))
			{
				writer.Write(json);
			}
		}
	}
}