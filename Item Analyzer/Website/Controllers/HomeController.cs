using System.Linq;
using System.Web.Mvc;

namespace Website.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string email, string password)
		{
			//if (string.IsNullOrWhiteSpace( email ))
			//	return null;

			//var securePassword = new SecureString();
			//password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			//var transport = new HttpTransport();

			//transport.Authenticate(email, securePassword, false);


			////Stream stream = transport.GetStash(1, "Standard", false);
			//var json = transport.GetStashJson(1, "Standard");

			//return new ContentResult
			//{
			//	Content = json
			//};

			return View();
		}

		public ActionResult LoadTabs(string email, string password)
		{
			return View();
		}
	}
}