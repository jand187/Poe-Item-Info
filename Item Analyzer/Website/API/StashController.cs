using System.Linq;
using System.Security;
using System.Web.Http;
using PoeItemInfo.Transport;

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

		public string Get(string email, string password, int stashTab)
		{
			var securePassword = new SecureString();
			password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			transport.Authenticate(email, securePassword, false);

			var json = transport.GetStashJson(stashTab, "Standard");

			return json;
		}
	}
}