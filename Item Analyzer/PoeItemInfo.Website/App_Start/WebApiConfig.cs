using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PoeItemInfo.Website
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new {action = "index", id = RouteParameter.Optional}
				);

			var jsonSetting = new JsonSerializerSettings();
			jsonSetting.Converters.Add(new StringEnumConverter());
			config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;

			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
		}
	}
}
