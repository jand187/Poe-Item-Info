using System.IO;
using System.Linq;
using System.Security;
using System.Web.Http;
using Newtonsoft.Json;
using PoeItemInfo.Data.Model.JSonProxy;
using PoeItemInfo.Model;
using PoeItemInfo.Transport;
using Website.Properties;

namespace Website.API
{
	public class TabListController : ApiController
	{
		private readonly IHttpTransport transport;

		public TabListController(IHttpTransport transport)
		{
			this.transport = transport;
		}

		public TabListController()
			: this(new HttpTransport())
		{
		}

		public dynamic Get(string email, string password)
		{
			var securePassword = new SecureString();
			password.ToCharArray().ToList().ForEach(securePassword.AppendChar);
			transport.Authenticate(email, securePassword, false);
			var json = transport.GetStashJson(0, "Standard");
			var stash = JsonConvert.DeserializeObject<Stash>(json);

			return new
			{
				Success = true,
				Tabs = stash.tabs.Select(t => new ItemTab
				{
					Index = t.i,
					Name = t.n,
					Loaded = GetFileInfo(t)
				}),
			};
		}

		private static string GetFileInfo(Tab tab)
		{
			var filename = Path.Combine(Settings.Default.DataDirectory, string.Format("Stash{0}.json", tab.i));
			var info = new FileInfo(filename);
			return info.Exists ? info.LastWriteTime.ToString("yyyy.MM.dd hh:mm:ss") : "never";
		}
	}
}