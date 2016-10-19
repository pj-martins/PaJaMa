using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace PaJaMa.Recipes.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.EnableCors();

            config.MapHttpAttributeRoutes();

            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;

			// Remove default XML handler
			var matches = config.Formatters
								.Where(f => f.SupportedMediaTypes
											 .Where(m => m.MediaType.ToString() == "application/xml" ||
														 m.MediaType.ToString() == "text/xml")
											 .Count() > 0)
								.ToList();
			foreach (var match in matches)
				config.Formatters.Remove(match);
        }
    }
}
