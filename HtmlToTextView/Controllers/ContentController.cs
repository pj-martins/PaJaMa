using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PaJaMa.HtmlToTextView.Controllers
{
    public class ContentController : ApiController
    {
		[HttpPost]
		public HttpResponseMessage GetText([FromBody]JObject url)
		{
			string text = string.Empty;
			using (var wc = new WebClient())
			{
				wc.UseDefaultCredentials = true;
				try
				{
					wc.Headers.Add("User-Agent: Other");
					text = wc.DownloadString(url["url"].ToString());
				}
				catch (WebException ex)
				{
					text = ex.Message;
					text += "\r\n\r\n";
					text += new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				}
				catch (Exception ex)
				{
					text = ex.Message;
				}
			}
			return Request.CreateResponse(HttpStatusCode.OK, text);
		}

		[HttpPost]
		public HttpResponseMessage Bing([FromBody]JObject keywords)
		{
			string text = string.Empty;
			using (var wc = new WebClient())
			{
				wc.UseDefaultCredentials = true;
				string url = "https://api.datamarket.azure.com/Bing/Search/v1/Web?$format=json&Query=%27" + System.Web.HttpUtility.UrlEncode(keywords["keywords"].ToString()) + "%27";
				wc.Headers.Add("Authorization", "Basic OmFZQWRBd0ZHV0pCZWxycnZkUjRMT1NGMkNFeDRSMmMyckFLZE5DSHIvOEE=");
				try
				{
					text = wc.DownloadString(url);
				}
				catch (WebException ex)
				{
					text = ex.Message;
					text += "\r\n\r\n";
					text += new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				}
				catch (Exception ex)
				{
					text = ex.Message;
				}
			}
			return Request.CreateResponse(HttpStatusCode.OK, text);
		}
    }
}
