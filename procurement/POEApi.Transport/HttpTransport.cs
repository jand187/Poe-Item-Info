using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using POEApi.Infrastructure;
using POEApi.Infrastructure.Events;

namespace POEApi.Transport
{
	public class HttpTransport : ITransport
	{
		private readonly string email;
		private readonly CookieContainer credentialCookies;

		private enum HttpMethod
		{
			GET,
			POST
		}

		private const string loginURL = @"https://www.pathofexile.com/login";
		private const string characterURL = @"http://www.pathofexile.com/character-window/get-characters";
		private const string stashURL = @"http://www.pathofexile.com/character-window/get-stash-items?league={0}&tabs=1&tabIndex={1}";
		private const string inventoryURL = @"http://www.pathofexile.com/character-window/get-items?character={0}";
		private const string hashRegEx = "name=\\\"hash\\\" value=\\\"(?<hash>[a-zA-Z0-9]{1,})\\\"";

		public event ThottledEventHandler Throttled;

		public HttpTransport(string login)
		{
			credentialCookies = new CookieContainer();
			email = login;
			RequestThrottle.Instance.Throttled += instance_Throttled;
		}

		private void instance_Throttled(object sender, ThottledEventArgs e)
		{
			if (Throttled != null)
				Throttled(this, e);
		}

		public bool Authenticate(string email, SecureString password, bool useSessionID)
		{
			if (useSessionID)
			{
				credentialCookies.Add(new Cookie("PHPSESSID", password.UnWrap(), "/", "www.pathofexile.com"));
				var confirmAuth = getHttpRequest(HttpMethod.GET, loginURL);
				var confirmAuthResponse = (HttpWebResponse) confirmAuth.GetResponse();

				if (confirmAuthResponse.ResponseUri.ToString() == loginURL)
					throw new LogonFailedException();
				return true;
			}

			var getHash = getHttpRequest(HttpMethod.GET, loginURL);
			var hashResponse = (HttpWebResponse) getHash.GetResponse();
			var loginResponse = Encoding.Default.GetString(getMemoryStreamFromResponse(hashResponse).ToArray());
			var hashValue = Regex.Match(loginResponse, hashRegEx).Groups["hash"].Value;

			var request = getHttpRequest(HttpMethod.POST, loginURL);
			request.AllowAutoRedirect = false;

			var data = new StringBuilder();
			data.Append("login_email=" + Uri.EscapeDataString(email));
			data.Append("&login_password=" + Uri.EscapeDataString(password.UnWrap()));
			data.Append("&hash=" + hashValue);

			var byteData = Encoding.UTF8.GetBytes(data.ToString());

			request.ContentLength = byteData.Length;

			var postStream = request.GetRequestStream();
			postStream.Write(byteData, 0, byteData.Length);

			var response = (HttpWebResponse) request.GetResponse();

			//If we didn't get a redirect, your gonna have a bad time.
			if (response.StatusCode != HttpStatusCode.Found)
				throw new LogonFailedException(this.email);

			return true;
		}

		private HttpWebRequest getHttpRequest(HttpMethod method, string url)
		{
			var request = (HttpWebRequest) RequestThrottle.Instance.Create(url);

			request.CookieContainer = credentialCookies;
			request.UserAgent =
				"User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 1.1.4322)";
			request.Method = method.ToString();
			request.ContentType = "application/x-www-form-urlencoded";

			return request;
		}

		public Stream GetStash(int index, string league, bool refresh)
		{
			var request = getHttpRequest(HttpMethod.GET, string.Format(stashURL, league, index));
			var response = (HttpWebResponse) request.GetResponse();

			return getMemoryStreamFromResponse(response);
		}

		public Stream GetCharacters()
		{
			var request = getHttpRequest(HttpMethod.GET, characterURL);
			var response = (HttpWebResponse) request.GetResponse();

			return getMemoryStreamFromResponse(response);
		}

		public Stream GetInventory(string characterName)
		{
			var request = getHttpRequest(HttpMethod.GET, string.Format(inventoryURL, characterName));
			var response = (HttpWebResponse) request.GetResponse();

			return getMemoryStreamFromResponse(response);
		}

		private MemoryStream getMemoryStreamFromResponse(HttpWebResponse response)
		{
			var reader = new StreamReader(response.GetResponseStream());
			var buffer = reader.ReadAllBytes();
			RequestThrottle.Instance.Complete();

			return new MemoryStream(buffer);
		}
	}
}