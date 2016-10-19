using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PaJaMa.HtmlToTextView
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
                routeTemplate: "api/{controller}/{action}"
            );

			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

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
