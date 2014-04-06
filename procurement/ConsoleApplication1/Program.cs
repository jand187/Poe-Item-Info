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
			var password = "";
			var email = "";

			if (args.Length == 0)
			{
				Console.Write("Enter email: ");
				email = Console.ReadLine();

				Console.Write("Enter password: ");
				password = Console.ReadLine();
			}

			var model = new POEModel();

			var securePassword = new SecureString();
			password.ToCharArray().ToList().ForEach(securePassword.AppendChar);
			model.Authenticate(email, securePassword, false, false);
			
			var stash = model.GetStash(0, "Standard", false);

			for (var index = 1; index < stash.NumberOfTabs; index++)
			{
				var stash2 = model.GetStash(index, "Standard", false);
			}

			Console.WriteLine(stash.Get<Item>().First().TypeLine);
			Console.ReadKey();
		}
	}
}
