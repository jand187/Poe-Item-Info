using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;
using Owin;
using RazorEngine;
using Server.Properties;
using Encoding = System.Text.Encoding;

namespace Server
{
	internal static class Program
	{
		/// <summary>
		///     The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			const string baseAddress = "http://localhost:9000/";

			using (WebApp.Start<Startup>(url: baseAddress))
			{
				Process.Start(baseAddress);
				Application.Run(new MyServerContext());
			}
		}
	}


	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			var config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new {id = RouteParameter.Optional}
				);

			config.Routes.MapHttpRoute(
				name: "DefaultWeb",
				routeTemplate: "{controller}/{id}",
				defaults: new {controller = "Home", id = RouteParameter.Optional}
				);

			appBuilder.UseWebApi(config);
		}
	}

	public class HomeController : ApiController
	{
		public HttpResponseMessage Get()
		{
			var model = new {Name = "World", Email = "someone@somewhere.com"};
			var content = Razor.Parse(File.ReadAllText(@"Views\Home\Index.cshtml"), model);

			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent(content, Encoding.UTF8, "text/html");
			return response;
		}
	}


	internal class MyServerContext : ApplicationContext
	{
		private readonly NotifyIcon icon;

		public MyServerContext()
		{
			icon = new NotifyIcon
			{
				Icon = new Icon(Resources.Server, 16, 16),
				Visible = true
			};

			var contextMenu = new ContextMenu();
			contextMenu.MenuItems.Add("Exit", ExitOnClick);
			icon.ContextMenu = contextMenu;
		}

		private void ExitOnClick(object sender, EventArgs eventArgs)
		{
			Application.Exit();
		}

		protected override void Dispose(bool disposing)
		{
			icon.Visible = false;
			base.Dispose(disposing);
		}
	}
}