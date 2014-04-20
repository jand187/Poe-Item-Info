using System.Linq;
using System.Security;
using System.Web.Mvc;
using PoeItemInfo.Transport;

namespace Website.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string email, string password)
		{
			var securePassword = new SecureString();
			password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			var transport = new HttpTransport(email);

			transport.Authenticate(email, securePassword, false);


			//Stream stream = transport.GetStash(1, "Standard", false);
			var json = transport.GetStashJson(1, "Standard");

			return new ContentResult
			{
				Content = json
			};
		}
	}
}