using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PaJaMa.Recipes.Web.Api.Controllers
{
#if DEBUG
	[System.Web.Http.Cors.EnableCors(origins: "*", headers: "*", methods: "*")]
#endif
	public class ImageController : ApiController
	{
		public HttpResponseMessage GetImage(string imageURL)
		{
			byte[] data = new WebClient().DownloadData(imageURL);
			var response = new HttpResponseMessage();
			response.Content = new StringContent(Convert.ToBase64String(data));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue(imageURL.EndsWith("png") ? "image/png" : "image/jpeg");
			return response;
		}
	}
}
