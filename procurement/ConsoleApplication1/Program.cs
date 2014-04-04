using System.Linq;
using System.Security;
using POEApi.Model;

namespace ConsoleApplication1
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var model = new POEModel();

			var password = new SecureString();
			"prutprutlugt".ToCharArray().ToList().ForEach(password.AppendChar);
			var authenticate = model.Authenticate("kimolsen@gmail.com", password, false, false);
			var chars = model.GetCharacters();
			var stash = model.GetStash(chars.First().League);
		}
	}
}
