//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using PaJaMa.Recipes.Web.Api.Models;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Web;
//using System.Web.Http;

//namespace PaJaMa.Recipes.Web.Api.Controllers
//{
//#if DEBUG
//	[System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
//#endif
//	public class EmailController : ApiController
//	{
//		public EmailController()
//		{
//		}

//		public HttpResponseMessage Get()
//		{
//			return new HttpResponseMessage(HttpStatusCode.Created);
//		}

//		[HttpPost]
//		public void Post(RecipeEmail recipeEmail)
//		{
//			// Create memory writer.
//			var sb = new StringBuilder();
//			var memWriter = new StringWriter(sb);

//			// Create fake http context to render the view.
//			var fakeResponse = new HttpResponse(memWriter);
//			var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request,
//				fakeResponse);
//			RecipeEmailTemplateController ctrller = new RecipeEmailTemplateController();
//			var routeData = new System.Web.Routing.RouteData();
//			routeData.Values.Add("Action", "Index");
//			routeData.Values.Add("Controller", "RecipeEmailTemplate");

//			var fakeControllerContext = new System.Web.Mvc.ControllerContext(
//				new HttpContextWrapper(fakeContext),
//				routeData,
//				ctrller);
//			var oldContext = System.Web.HttpContext.Current;
//			System.Web.HttpContext.Current = fakeContext;

//			// Render the view.
//			ctrller.RecipeEmailOutput(recipeEmail).ExecuteResult(fakeControllerContext);

//			// Restore old context.
//			System.Web.HttpContext.Current = oldContext;

//			// Flush memory and return output.
//			memWriter.Flush();

//			string[] tos = recipeEmail.To.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);

//			PaJaMa.Common.EMail.Send("smtp.gmail.com", 587, true, "pj.martins@gmail.com", "/Psalms123", new System.Net.Mail.MailAddress("pj.martins@gmail.com", "Recipe Searcher"),
//				tos.Select(t => new System.Net.Mail.MailAddress(t)).ToList(), recipeEmail.Subject, sb.ToString(), true, null);
//		}
//	}
//}
