using System;
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
			model.Authenticate("kimolsen@gmail.com", password, false, false);
			
			var stash = model.GetStash(0, "Standard", false);

			Console.WriteLine(stash.Get<Item>().First().TypeLine);
			Console.ReadKey();
		}
	}
}
