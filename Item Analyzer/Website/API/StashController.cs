using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using PoeItemInfo.Transport;

namespace Website.API
{
    public class StashController : ApiController
    {


		public string Get(string email, string password, int stashTab)
		{
			var securePassword = new SecureString();
			password.ToCharArray().ToList().ForEach(securePassword.AppendChar);

			var transport = new HttpTransport(email);

			transport.Authenticate(email, securePassword, false);

			var json = transport.GetStashJson(stashTab, "Standard");

			return json;
		}
	}
}
