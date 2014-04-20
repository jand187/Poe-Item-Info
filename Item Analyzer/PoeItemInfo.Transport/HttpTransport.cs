using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace PoeItemInfo.Transport
{
	public interface IHttpTransport
	{
		bool Authenticate(string email, SecureString securePassword, bool useSessionId);
		string GetStashJson(int index, string league);
	}

	public class HttpTransport : IHttpTransport
	{
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

		public HttpTransport()
		{
			credentialCookies = new CookieContainer();
			RequestThrottle.Instance.Throttled += InstanceThrottled;
		}

		private void InstanceThrottled(object sender, ThottledEventArgs e)
		{
			if (Throttled != null)
				Throttled(this, e);
		}

		public bool Authenticate(string email, SecureString password, bool useSessionId)
		{
			var getHash = GetHttpRequest(HttpMethod.GET, loginURL);
			var hashResponse = (HttpWebResponse) getHash.GetResponse();
			var loginResponse = Encoding.Default.GetString(GetMemoryStreamFromResponse(hashResponse).ToArray());
			var hashValue = Regex.Match(loginResponse, hashRegEx).Groups["hash"].Value;

			var request = GetHttpRequest(HttpMethod.POST, loginURL);
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

			if (response.StatusCode != HttpStatusCode.Found)
				throw new Exception();

			return true;
		}

		private HttpWebRequest GetHttpRequest(HttpMethod method, string url)
		{
			var request = (HttpWebRequest) RequestThrottle.Instance.Create(url);

			request.CookieContainer = credentialCookies;
			request.UserAgent =
				"User-Agent: Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E; .NET CLR 1.1.4322)";
			request.Method = method.ToString();
			request.ContentType = "application/x-www-form-urlencoded";

			return request;
		}

		public string GetStashJson(int index, string league)
		{
			var request = GetHttpRequest(HttpMethod.GET, string.Format(stashURL, league, index));
			var response = (HttpWebResponse)request.GetResponse();
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				return reader.ReadToEnd();
			}
		}


		//public Stream GetStash(int index, string league, bool refresh)
		//{
		//	var request = GetHttpRequest(HttpMethod.POST, string.Format(stashURL, league, index));
		//	var response = (HttpWebResponse) request.GetResponse();

		//	var stream = GetMemoryStreamFromResponse(response);

		//	var reader = new StreamReader(stream);

		//	var contents = reader.ReadToEnd();

		//	var dir = Directory.CreateDirectory(@".\data");
		//	var filename = string.Format("tab{0}.json", index);
		//	var fullFilename = Path.Combine(dir.FullName, filename);
		//	using (var writer = File.CreateText(fullFilename))
		//	{
		//		writer.Write(contents);
		//		Console.WriteLine(string.Format("Created: {0}", filename));
		//	}

		//	stream.Position = 0;
		//	return stream;
		//}

		public Stream GetCharacters()
		{
			var request = GetHttpRequest(HttpMethod.GET, characterURL);
			var response = (HttpWebResponse) request.GetResponse();

			return GetMemoryStreamFromResponse(response);
		}

		public Stream GetInventory(string characterName)
		{
			var request = GetHttpRequest(HttpMethod.GET, string.Format(inventoryURL, characterName));
			var response = (HttpWebResponse) request.GetResponse();

			return GetMemoryStreamFromResponse(response);
		}

		private MemoryStream GetMemoryStreamFromResponse(HttpWebResponse response)
		{
			var reader = new StreamReader(response.GetResponseStream());
			var buffer = reader.ReadAllBytes();
			RequestThrottle.Instance.Complete();

			return new MemoryStream(buffer);
		}
	}
}