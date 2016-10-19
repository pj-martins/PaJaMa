using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PaJaMa.Common
{
	//public class WebCommon
	//{
	//	static private Assembly GetWebEntryAssembly()
	//	{
	//		if (System.Web.HttpContext.Current == null ||
	//			System.Web.HttpContext.Current.ApplicationInstance == null)
	//		{
	//			return null;
	//		}

	//		var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
	//		while (type != null && type.Namespace == "ASP")
	//		{
	//			type = type.BaseType;
	//		}

	//		return type == null ? null : type.Assembly;
	//	}
	//}

	public class GZipWebClient : WebClient
	{
		protected override WebRequest GetWebRequest(Uri address)
		{
			var request = base.GetWebRequest(address) as HttpWebRequest;
			if (request == null) throw new InvalidOperationException("You cannot use this WebClient implementation with an address that is not an http uri.");
			request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			return request;
		}
	}
}
